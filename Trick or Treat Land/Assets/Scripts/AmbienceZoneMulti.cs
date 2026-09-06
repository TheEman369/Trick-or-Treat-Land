using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; // only needed if you want to route to a mixer group

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class AmbienceZoneMulti : MonoBehaviour
{
    [Header("Area + Players")]
    [Tooltip("Collider that defines this ambience area (defaults to this object's Collider).")]
    public Collider area;

    [Tooltip("All player pieces. If empty, we'll try to auto-find by tag 'Player'.")]
    public List<Transform> players = new List<Transform>();

    [Tooltip("If true, only track this player index from the list (0-based). If false, uses nearest player.")]
    public bool useActivePlayerOnly = false;
    [Min(0)] public int activePlayerIndex = 0;

    [Header("Audio")]
    [Tooltip("Looping ambience clip for this zone.")]
    public AudioClip ambientClip;

    [Range(0f, 1f)] public float targetVolume = 0.6f;
    [Tooltip("Seconds to fade when entering the zone.")]
    public float fadeInSeconds = 0.75f;
    [Tooltip("Seconds to fade when leaving the zone.")]
    public float fadeOutSeconds = 0.5f;

    [Tooltip("Optional: route this source to your Ambience/SFX mixer group.")]
    public AudioMixerGroup outputGroup;

    private AudioSource src;
    private Coroutine fadeRoutine;
    private Transform chosenPlayer;     // whichever player we’re tracking this frame
    private bool isInside;              // was the chosen player inside last frame?

    private void Awake()
    {
        // Cache components
        src = GetComponent<AudioSource>();
        if (!area) area = GetComponent<Collider>();

        // Configure AudioSource for 3D ambience
        src.clip = ambientClip;
        src.loop = true;
        src.playOnAwake = false;
        src.spatialBlend = 1f;                 // 3D
        src.rolloffMode = AudioRolloffMode.Logarithmic;
        src.minDistance = 3f;                  // tune for your board scale
        src.maxDistance = 25f;
        if (outputGroup) src.outputAudioMixerGroup = outputGroup;

        // Fill players list if empty by tag
        if (players.Count == 0)
        {
            var found = GameObject.FindGameObjectsWithTag("Player");
            foreach (var go in found) players.Add(go.transform);
        }
    }

    private void Update()
    {
        // Choose which player to track (active only or nearest)
        chosenPlayer = ResolvePlayer();
        if (!chosenPlayer || !area) return;

        // Compute closest point on the zone to that player
        Vector3 closestPoint = area.ClosestPoint(chosenPlayer.position);

        // Move the ambience source to that closest point (good spatial cue near walls/edges)
        transform.position = closestPoint;

        // Detect if the player is inside:
        // When inside, ClosestPoint returns the player's position, so distance ≈ 0.
        bool nowInside = (closestPoint - chosenPlayer.position).sqrMagnitude < 0.000001f;

        // Handle enter/exit with fades
        if (nowInside && !isInside)
        {
            StartFade(targetVolume, fadeInSeconds, ensurePlaying: true);
        }
        else if (!nowInside && isInside)
        {
            StartFade(0f, fadeOutSeconds, ensurePlaying: false);
        }

        isInside = nowInside;
    }

    /// <summary>
    /// Picks which player to follow: either a specific one (active index) or the nearest.
    /// </summary>
    private Transform ResolvePlayer()
    {
        if (players == null || players.Count == 0) return null;

        if (useActivePlayerOnly)
        {
            int idx = Mathf.Clamp(activePlayerIndex, 0, players.Count - 1);
            return players[idx];
        }

        // Nearest player to the area bounds (using distance to ClosestPoint)
        Transform best = null;
        float bestSqr = float.PositiveInfinity;

        foreach (var p in players)
        {
            if (!p) continue;
            Vector3 cp = area.ClosestPoint(p.position);
            float d2 = (cp - p.position).sqrMagnitude;
            if (d2 < bestSqr) { bestSqr = d2; best = p; }
        }
        return best;
    }

    private void StartFade(float toVol, float seconds, bool ensurePlaying)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(toVol, seconds, ensurePlaying));
    }

    private IEnumerator FadeTo(float toVol, float seconds, bool ensurePlaying)
    {
        if (ensurePlaying && !src.isPlaying) src.Play();

        float fromVol = src.volume;
        float t = 0f;

        // Protect against 0-second fades
        seconds = Mathf.Max(0.0001f, seconds);

        while (t < seconds)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(fromVol, toVol, t / seconds);
            yield return null;
        }

        src.volume = toVol;

        if (!ensurePlaying && Mathf.Approximately(toVol, 0f))
        {
            src.Stop();
        }
    }
}
