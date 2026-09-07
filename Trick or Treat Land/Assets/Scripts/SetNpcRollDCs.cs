#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class SetNpcRollDCs
{
    [MenuItem("Tools/Dialogue/Set NPC Roll DCs")]
    public static void Run()
    {
        Apply("Frankenmellow Monster", 8);
        Apply("SourWereWolf", 9);
        Apply("Licorice Witch", 12);
        Apply("Gummy Mummy", 12);
        Apply("Choco Creature", 14);
        Apply("Count Suckerla", 15);

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("NPC DCs", "Updated all NPC DCs.", "OK");
    }

    static void Apply(string npcName, int dc)
    {
        string[] guids = AssetDatabase.FindAssets($"t:NpcDialogue NPC_{npcName.Replace(" ", "")}");
        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var npc = AssetDatabase.LoadAssetAtPath<NpcDialogue>(path);
            if (npc == null) continue;

            foreach (var sc in npc.scenes)
            {
                if (sc == null) continue;
                if (sc.sceneId == "trick_roll" || sc.sceneId == "treat_roll")
                {
                    sc.requiresRoll = true;
                    sc.rollDC = dc;
                }
            }

            EditorUtility.SetDirty(npc);
            Debug.Log($"Set DC {dc} for {npc.displayName}");
        }
    }
}
#endif
