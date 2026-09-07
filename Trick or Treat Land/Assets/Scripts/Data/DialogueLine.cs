using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 6)] public string text;
    public AudioClip voice;          // optional
    public float autoAdvanceAfter = 0f; // 0 = wait for click
}
