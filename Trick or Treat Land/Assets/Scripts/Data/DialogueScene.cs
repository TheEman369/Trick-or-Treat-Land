using UnityEngine;

[System.Serializable]
public class DialogueScene
{
    public string sceneId;                 // e.g. "intro", "trick_roll", "trick_success"
    public Sprite portraitOverride;        // optional override portrait
    public DialogueLine[] lines;

    // Choice UI (optional)
    public bool hasChoice = false;
    public string choiceA = "Trick";
    public string choiceB = "Treat";
    public string nextOnChoiceA;           // e.g. "trick_roll"
    public string nextOnChoiceB;           // e.g. "treat_roll"

    // Roll gate (optional)
    public bool requiresRoll = false;
    public int rollDC = 12;
    public string nextOnSuccess;           // e.g. "trick_success"
    public string nextOnFail;              // e.g. "trick_fail"

    // Effect hint for coder (data-only, no gameplay here)
    public enum EffectType { None, Move, GainCandy, LoseTurn, MissTurn, RollAgain, Stay }
    public EffectType effect = EffectType.None;
    public int effectValue = 0;            // e.g., Move +3, Move -1, LoseTurn 1, etc.
    [TextArea(1, 4)] public string effectNote;
}
