using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class AmbienceZone : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Assign your Player transform here or leave empty to find by tag 'Player'.")]
    [SerializeField] private Transform player;

    [Tooltip("Optional: explicitly set the area collider. Defaults to this GameObject's Collider.")]
    [SerializeField] private Collider area;

    [Header("Audio")]
    [SerializeField] private AudioClip ambientClip;   // your ambient loop
    [SerializeField, Range(0f, 1f)] private float targetVolume = 0.6f;
    [SerializeField] private float fadeInSeconds = 0.75f;
    [SerializeField] private float fadeOutSeconds = 0.5f;

    private AudioSource src;
    private Coroutine fadeRoutine;
    private bool playerInside;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        if (!area) area = GetComponent<Collider>();

        // Ensure trigger behavior
        if (area) area.isTrigger = true;

        // Configure the AudioSource for spatial ambience
        src.clip = ambientClip;
        src.loop = true;
        src.playOnAwake = false;
        src.spatialBlend = 1f;           // 3D
        src.rolloffMode = AudioRolloffMode.Logarithmic;
        src.minDistance = 3f;            // tune per zone size
        src.maxDistance = 25f;           // tune per zone size

        if (!player)
        {
            var tagged = GameObject.FindGameObjectWithTag("Player");
            if (tagged) player = tagged.transform;
        }
    }

    private void Update()
    {
        // Follow closest point so the sound seems to 'come from the wall/edge' of the zone
        if (player && area)
        {
            Vector3 closestPoint = area.ClosestPoint(player.position);
            transform.position = closestPoint;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!player) return;
        if (other.transform != player) return;

        playerInside = true;
        StartFadeIn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!player) return;
        if (other.transform != player) return;

        playerInside = false;
        StartFadeOut();
    }

    private void StartFadeIn()
    {
        if (!ambientClip) { Debug.LogWarning("AmbienceZone: No ambientClip assigned."); return; }
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(targetVolume, fadeInSeconds, ensurePlaying: true));
    }

    private void StartFadeOut()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(0f, fadeOutSeconds, ensurePlaying: false));
    }

    private IEnumerator FadeTo(float vol, float seconds, bool ensurePlaying)
    {
        if (ensurePlaying && !src.isPlaying) src.Play();

        float start = src.volume;
        float t = 0f;

        while (t < seconds)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(start, vol, t / seconds);
            yield return null;
        }
        src.volume = vol;

        if (!ensurePlaying && Mathf.Approximately(vol, 0f))
            src.Stop();
    }
}
