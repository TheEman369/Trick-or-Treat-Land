using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Routing")]
    [SerializeField] private AudioMixer audioMixer;             // same mixer as SoundMixerManager
    [SerializeField] private string musicParam = "musicVolume"; // must match exposed param name

    [Header("Source")]
    [SerializeField] private AudioSource musicSource;           // assign in Inspector (loop ON, playOnAwake OFF)

    [Header("Defaults")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    // typical mixer ranges are in dB. e.g., -80 (silent) to 0 (full)
    [SerializeField] private float minDb = -80f;
    [SerializeField] private float maxDb = 0f;

    private void Awake()
    {
        // Singleton pattern — keep only one across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure we have a usable AudioSource
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.outputAudioMixerGroup = null; // optionally assign a Music group in the Inspector
        }
    }

    public void PlayMusic(AudioClip clip, float fadeSeconds = 0.5f)
    {
        if (clip == null) return;

        // If same clip already playing, do nothing
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        StopAllCoroutines();
        StartCoroutine(CrossfadeTo(clip, fadeSeconds));
    }

    public void StopMusic(float fadeSeconds = 0.5f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeToDb(minDb, fadeSeconds, stopAfterFade: true));
    }

    public void PlayMenuMusic(float fadeSeconds = 0.5f) => PlayMusic(menuMusic, fadeSeconds);
    public void PlayGameMusic(float fadeSeconds = 0.5f) => PlayMusic(gameMusic, fadeSeconds);

    private IEnumerator CrossfadeTo(AudioClip nextClip, float fadeSeconds)
    {
        // Fade down current
        yield return FadeToDb(minDb, fadeSeconds, stopAfterFade: false);

        // Swap clip and start
        musicSource.clip = nextClip;
        musicSource.Play();

        // Fade up to full
        yield return FadeToDb(maxDb, fadeSeconds, stopAfterFade: false);
    }

    private IEnumerator FadeToDb(float targetDb, float seconds, bool stopAfterFade)
    {
        // Read current dB (fallback to max if not set yet)
        audioMixer.GetFloat(musicParam, out float startDb);
        if (float.IsNaN(startDb)) startDb = maxDb;

        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime; // unaffected by Time.timeScale
            float db = Mathf.Lerp(startDb, targetDb, seconds > 0f ? t / seconds : 1f);
            audioMixer.SetFloat(musicParam, db);
            yield return null;
        }
        audioMixer.SetFloat(musicParam, targetDb);

        if (stopAfterFade && musicSource.isPlaying)
            musicSource.Stop();
    }
}
