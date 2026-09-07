using System.Collections;
using UnityEngine;

public class MusicDirector : MonoBehaviour
{
    public static MusicDirector Instance { get; private set; }

    [Header("Global BGM Source (2D)")]
    [SerializeField] private AudioSource bgmSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip, float volume = 1f, float fade = 0.5f)
    {
        if (!bgmSource) return;
        StopAllCoroutines();
        StartCoroutine(FadeIn(clip, volume, fade));
    }

    public void StopBGM(float fade = 0.5f)
    {
        if (!bgmSource) return;
        StopAllCoroutines();
        StartCoroutine(FadeOut(fade));
    }

    private IEnumerator FadeIn(AudioClip clip, float targetVol, float fade)
    {
        bgmSource.clip = clip;
        bgmSource.volume = 0f;
        bgmSource.loop = true;
        bgmSource.Play();

        float t = 0f;
        while (t < fade)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVol, t / fade);
            yield return null;
        }
        bgmSource.volume = targetVol;
    }

    private IEnumerator FadeOut(float fade)
    {
        float start = bgmSource.volume;
        float t = 0f;
        while (t < fade)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(start, 0f, t / fade);
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.clip = null;
    }
}