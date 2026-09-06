#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using static DialogueScene; // for EffectType

public static class GenerateHalloweenNpcs
{
    [MenuItem("Assets/Create/Dialogue/Generate Halloween NPCs")]
    public static void Run()
    {
        const string root = "Assets/Dialogue/NPCs";
        if (!AssetDatabase.IsValidFolder("Assets/Dialogue"))
            AssetDatabase.CreateFolder("Assets", "Dialogue");
        if (!AssetDatabase.IsValidFolder(root))
            AssetDatabase.CreateFolder("Assets/Dialogue", "NPCs");

        Make_CountSuckerla(root);        // DC 15
        Make_Frankenmellow(root);        // DC 8
        Make_SourWereWolf(root);         // DC 9
        Make_LicoriceWitch(root);        // DC 12
        Make_GummyMummy(root);           // DC 12
        Make_ChocoCreature(root);        // DC 14

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("NPCs", "Generated 6 Halloween NPC dialogue assets.", "OK");
    }

    static DialogueLine L(string text) => new DialogueLine { text = text };

    static NpcDialogue Make(string root, NpcId id, string displayName, DialogueScene[] scenes)
    {
        var npc = ScriptableObject.CreateInstance<NpcDialogue>();
        npc.id = id;
        npc.displayName = displayName;
        npc.scenes = scenes;

        var path = $"{root}/NPC_{Sanitize(displayName)}.asset";
        AssetDatabase.CreateAsset(npc, path);
        return npc;
    }

    static string Sanitize(string s)
    {
        foreach (char c in Path.GetInvalidFileNameChars()) s = s.Replace(c.ToString(), "");
        return s.Replace(" ", "");
    }

    // ---------- Count Suckerla (DC 15) ----------
    static void Make_CountSuckerla(string root)
    {
        int DC = 15;

        var intro = new DialogueScene
        {
            sceneId = "intro",
            lines = new[] { L("Ah, velcome, little morsel… I mean, traveler. I vant to suck-er on these candies all night, but I promise I’ll only take one bite. Life can be sweet… but sometimes it really bites! Don’t worry, my jokes aren’t as bad as my sweet tooth. Now tell me, will it be Trick or Treat?") },
            hasChoice = true,
            choiceA = "Trick",
            choiceB = "Treat",
            nextOnChoiceA = "trick_roll",
            nextOnChoiceB = "treat_roll"
        };

        var trickRoll = new DialogueScene
        {
            sceneId = "trick_roll",
            lines = new[] { L("Let us see if the bats favor you…") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "trick_success",
            nextOnFail = "trick_fail"
        };
        var trickSuccess = new DialogueScene
        {
            sceneId = "trick_success",
            lines = new[] { L("His bats swirl into candy shapes. “Looks like I really sucked you in—enjoy a bonus snack!”") },
            effect = EffectType.GainCandy,
            effectValue = 1,
            effectNote = "Gain extra candy"
        };
        var trickFail = new DialogueScene
        {
            sceneId = "trick_fail",
            lines = new[] { L("The bats drop candy instead of scaring you. “Drat! Foiled again by a sweet fang-tasy.”") },
            effect = EffectType.Move,
            effectValue = +2,
            effectNote = "Move forward 2"
        };

        var treatRoll = new DialogueScene
        {
            sceneId = "treat_roll",
            lines = new[] { L("Behold my glowing jewel… if you are worthy.") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "treat_success",
            nextOnFail = "treat_fail"
        };
        var treatSuccess = new DialogueScene
        {
            sceneId = "treat_success",
            lines = new[] { L("His glowing jewel boosts you. “Now that’s a real lifesaver!”") },
            effect = EffectType.Move,
            effectValue = +3,
            effectNote = "Move forward 3"
        };
        var treatFail = new DialogueScene
        {
            sceneId = "treat_fail",
            lines = new[] { L("The jewel melts into sticky goo. “Guess that treat was a little too hard to swallow.”") },
            effect = EffectType.LoseTurn,
            effectValue = 1,
            effectNote = "Lose a turn"
        };

        Make(root, NpcId.CountSuckerla, "Count Suckerla", new[] { intro, trickRoll, trickSuccess, trickFail, treatRoll, treatSuccess, treatFail });
    }

    // ---------- Frankenmellow Monster (DC 8) ----------
    static void Make_Frankenmellow(string root)
    {
        int DC = 8;

        var intro = new DialogueScene
        {
            sceneId = "intro",
            lines = new[] { L("Grraahh… I was going to be frightening, but I just don’t have the s’more-ale for it. My castle keeps collapsing—guess it wasn’t built on a very solid foundation! Sticky situations are kinda my specialty, though. So, before I goo too far… what’s it gonna be, Trick or Treat?") },
            hasChoice = true,
            choiceA = "Trick",
            choiceB = "Treat",
            nextOnChoiceA = "trick_roll",
            nextOnChoiceB = "treat_roll"
        };

        var trickRoll = new DialogueScene
        {
            sceneId = "trick_roll",
            lines = new[] { L("Splat test!") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "trick_success",
            nextOnFail = "trick_fail"
        };
        var trickSuccess = new DialogueScene
        {
            sceneId = "trick_success",
            lines = new[] { L("Marshmallow goo sticks you. “Splat! You’re really in a sticky situation now.”") },
            effect = EffectType.LoseTurn,
            effectValue = 1,
            effectNote = "Lose a turn"
        };
        var trickFail = new DialogueScene
        {
            sceneId = "trick_fail",
            lines = new[] { L("The goo bounces you forward. “Whoops—looks like you got a real marsh-boost!”") },
            effect = EffectType.Move,
            effectValue = +2,
            effectNote = "Move forward 2"
        };

        var treatRoll = new DialogueScene
        {
            sceneId = "treat_roll",
            lines = new[] { L("Share a marsh-brick? Let’s see…") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "treat_success",
            nextOnFail = "treat_fail"
        };
        var treatSuccess = new DialogueScene
        {
            sceneId = "treat_success",
            lines = new[] { L("He shares a marshmallow brick. “That’s one solid mallow move!”") },
            effect = EffectType.Move,
            effectValue = +3,
            effectNote = "Move forward 3"
        };
        var treatFail = new DialogueScene
        {
            sceneId = "treat_fail",
            lines = new[] { L("The brick crumbles in your hands. “Guess that was a half-baked idea.”") },
            effect = EffectType.Stay,
            effectValue = 0,
            effectNote = "Stay put"
        };

        Make(root, NpcId.FrankenmellowMonster, "Frankenmellow Monster", new[] { intro, trickRoll, trickSuccess, trickFail, treatRoll, treatSuccess, treatFail });
    }

    // ---------- Sour-were Wolf (DC 9) ----------
    static void Make_SourWereWolf(string root)
    {
        int DC = 9;

        var intro = new DialogueScene
        {
            sceneId = "intro",
            lines = new[] { L("Awooo! Careful, traveler—my mood can be sweet or sour, depending on the moon. Don’t worry, I won’t bite… well, maybe just a gummy nibble. Sometimes I’m a real howl, other times I’m just a chew toy for puns. So, brave one, do you dare take a Trick or Treat?") },
            hasChoice = true,
            choiceA = "Trick",
            choiceB = "Treat",
            nextOnChoiceA = "trick_roll",
            nextOnChoiceB = "treat_roll"
        };

        var trickRoll = new DialogueScene
        {
            sceneId = "trick_roll",
            lines = new[] { L("Sour spray challenge!") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "trick_success",
            nextOnFail = "trick_fail"
        };
        var trickSuccess = new DialogueScene
        {
            sceneId = "trick_success",
            lines = new[] { L("Sour spray burns your tongue. “Yikes! That really puts the sour in power.”") },
            effect = EffectType.Move,
            effectValue = -1,
            effectNote = "Move back 1"
        };
        var trickFail = new DialogueScene
        {
            sceneId = "trick_fail",
            lines = new[] { L("The sour spray refreshes you. “Awooo! Guess you like the tangy twist.”") },
            effect = EffectType.GainCandy,
            effectValue = 1,
            effectNote = "Gain extra candy"
        };

        var treatRoll = new DialogueScene
        {
            sceneId = "treat_roll",
            lines = new[] { L("Try this gummy fang…") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "treat_success",
            nextOnFail = "treat_fail"
        };
        var treatSuccess = new DialogueScene
        {
            sceneId = "treat_success",
            lines = new[] { L("The gummy fang boosts you. “Now you’re running with the pack!”") },
            effect = EffectType.RollAgain,
            effectValue = 1,
            effectNote = "Roll again"
        };
        var treatFail = new DialogueScene
        {
            sceneId = "treat_fail",
            lines = new[] { L("The fang gets stuck in your teeth. “Talk about a chewy problem.”") },
            effect = EffectType.MissTurn,
            effectValue = 1,
            effectNote = "Miss next turn"
        };

        Make(root, NpcId.SourWereWolf, "Sour-were Wolf", new[] { intro, trickRoll, trickSuccess, trickFail, treatRoll, treatSuccess, treatFail });
    }

    // ---------- Licorice Witch (DC 12) ----------
    static void Make_LicoriceWitch(string root)
    {
        int DC = 12;

        var intro = new DialogueScene
        {
            sceneId = "intro",
            lines = new[] { L("Cackle, cackle! My cauldron bubbles with red twists and black swirls. I tried to make a potion of invisibility… but it turned out to be licorice! Ha! Don’t worry, my rhymes are twisted, but my humor is even stickier. Come closer, traveler, and pick your fate—Trick or Treat?") },
            hasChoice = true,
            choiceA = "Trick",
            choiceB = "Treat",
            nextOnChoiceA = "trick_roll",
            nextOnChoiceB = "treat_roll"
        };

        var trickRoll = new DialogueScene
        {
            sceneId = "trick_roll",
            lines = new[] { L("Tangled or lucky? Let’s see.") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "trick_success",
            nextOnFail = "trick_fail"
        };
        var trickSuccess = new DialogueScene
        {
            sceneId = "trick_success",
            lines = new[] { L("Licorice tangles your feet. “Ha! You’ve been twisted up in knots.”") },
            effect = EffectType.LoseTurn,
            effectValue = 1,
            effectNote = "Lose 1 turn"
        };
        var trickFail = new DialogueScene
        {
            sceneId = "trick_fail",
            lines = new[] { L("The licorice untangles into a rope. “Looks like you pulled the sweet end of the stick!”") },
            effect = EffectType.Move,
            effectValue = +2,
            effectNote = "Move forward 2"
        };

        var treatRoll = new DialogueScene
        {
            sceneId = "treat_roll",
            lines = new[] { L("Hop on my broom—if it holds…") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "treat_success",
            nextOnFail = "treat_fail"
        };
        var treatSuccess = new DialogueScene
        {
            sceneId = "treat_success",
            lines = new[] { L("Her licorice broomstick zooms you ahead. “That’s what I call a sweet ride!”") },
            effect = EffectType.Move,
            effectValue = +3,
            effectNote = "Move forward 3"
        };
        var treatFail = new DialogueScene
        {
            sceneId = "treat_fail",
            lines = new[] { L("The broomstick snaps in half. “Guess it wasn’t built to last a-licorice.”") },
            effect = EffectType.Stay,
            effectValue = 0,
            effectNote = "Stay in place"
        };

        Make(root, NpcId.LicoriceWitch, "Licorice Witch", new[] { intro, trickRoll, trickSuccess, trickFail, treatRoll, treatSuccess, treatFail });
    }

    // ---------- Gummy Mummy (DC 12) ----------
    static void Make_GummyMummy(string root)
    {
        int DC = 12;

        var intro = new DialogueScene
        {
            sceneId = "intro",
            lines = new[] { L("Wrapped up again… talk about a sticky situation! I’ve been guarding sweets for a thousand years—guess I’m really preserved. My jokes might be ancient, but at least they don’t fall apart like my bandages. So unwrap this riddle of sugar and fright—Trick or Treat?") },
            hasChoice = true,
            choiceA = "Trick",
            choiceB = "Treat",
            nextOnChoiceA = "trick_roll",
            nextOnChoiceB = "treat_roll"
        };

        var trickRoll = new DialogueScene
        {
            sceneId = "trick_roll",
            lines = new[] { L("Bandage trial!") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "trick_success",
            nextOnFail = "trick_fail"
        };
        var trickSuccess = new DialogueScene
        {
            sceneId = "trick_success",
            lines = new[] { L("Gummy wraps tie you tight. “Looks like you’ve been preserved for later!”") },
            effect = EffectType.Move,
            effectValue = -2,
            effectNote = "Move back 2"
        };
        var trickFail = new DialogueScene
        {
            sceneId = "trick_fail",
            lines = new[] { L("The wraps spring you forward. “You’ve been unwrapped to victory!”") },
            effect = EffectType.Move,
            effectValue = +2,
            effectNote = "Move forward 2"
        };

        var treatRoll = new DialogueScene
        {
            sceneId = "treat_roll",
            lines = new[] { L("Do you deserve this gummy gem?") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "treat_success",
            nextOnFail = "treat_fail"
        };
        var treatSuccess = new DialogueScene
        {
            sceneId = "treat_success",
            lines = new[] { L("You unwrap a gummy gem. “An ancient chew-reasure!”") },
            effect = EffectType.GainCandy,
            effectValue = 1,
            effectNote = "Gain bonus candy"
        };
        var treatFail = new DialogueScene
        {
            sceneId = "treat_fail",
            lines = new[] { L("The gem turns into sticky sludge. “That’s a mummy’s curse for you.”") },
            effect = EffectType.LoseTurn,
            effectValue = 1,
            effectNote = "Lose a turn"
        };

        Make(root, NpcId.GummyMummy, "Gummy Mummy", new[] { intro, trickRoll, trickSuccess, trickFail, treatRoll, treatSuccess, treatFail });
    }

    // ---------- Choco Creature from the Chocolate Swamp (DC 14) ----------
    static void Make_ChocoCreature(string root)
    {
        int DC = 14;

        var intro = new DialogueScene
        {
            sceneId = "intro",
            lines = new[] { L("Glorp… welcome to my swamp—some say I’m a real choc-olate case. Every step here is fudgier than the last, but don’t worry—I won’t dessert you. Things can get a little rocky road down here, but that’s half the fun! Now tell me, candy traveler—will you risk the swamp’s Trick or Treat?") },
            hasChoice = true,
            choiceA = "Trick",
            choiceB = "Treat",
            nextOnChoiceA = "trick_roll",
            nextOnChoiceB = "treat_roll"
        };

        var trickRoll = new DialogueScene
        {
            sceneId = "trick_roll",
            lines = new[] { L("The caramel rises…") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "trick_success",
            nextOnFail = "trick_fail"
        };
        var trickSuccess = new DialogueScene
        {
            sceneId = "trick_success",
            lines = new[] { L("Caramel pulls you down. “Sorry, this is a real sticky ending.”") },
            effect = EffectType.MissTurn,
            effectValue = 1,
            effectNote = "Miss 1 turn"
        };
        var trickFail = new DialogueScene
        {
            sceneId = "trick_fail",
            lines = new[] { L("The swamp spits you out. “Guess you’re just too sweet to sink.”") },
            effect = EffectType.Move,
            effectValue = +2,
            effectNote = "Move forward 2"
        };

        var treatRoll = new DialogueScene
        {
            sceneId = "treat_roll",
            lines = new[] { L("A chocolate coin… if it holds.") },
            requiresRoll = true,
            rollDC = DC,
            nextOnSuccess = "treat_success",
            nextOnFail = "treat_fail"
        };
        var treatSuccess = new DialogueScene
        {
            sceneId = "treat_success",
            lines = new[] { L("Chocolate coin boosts you. “Cha-ching! That’s some real candy capital.”") },
            effect = EffectType.Move,
            effectValue = +3,
            effectNote = "Move forward 3"
        };
        var treatFail = new DialogueScene
        {
            sceneId = "treat_fail",
            lines = new[] { L("The coin melts in your hand. “Looks like your fortune just melted away.”") },
            effect = EffectType.Stay,
            effectValue = 0,
            effectNote = "Stay put"
        };

        Make(root, NpcId.ChocoCreature, "Choco Creature from the Chocolate Swamp", new[] { intro, trickRoll, trickSuccess, trickFail, treatRoll, treatSuccess, treatFail });
    }
}
#endif
