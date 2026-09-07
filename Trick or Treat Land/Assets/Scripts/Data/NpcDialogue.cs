using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NPC Dialogue", fileName = "NPC_")]
public class NpcDialogue : ScriptableObject
{
    public NpcId id;
    public string displayName;
    public Sprite defaultPortrait;     // optional
    public AudioClip theme;            // optional
    public DialogueScene[] scenes;
}
