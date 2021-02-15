using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceDialogue : MonoBehaviour
{
    public static int GetDialogueCount(int id)
    {
        switch(id)
        {
            case 1: return AdvanceSusan();
            case 2: return AdvanceApril();
            case 3: return AdvanceMom();
        }

        return 0;
    }

    // 1: Susan
    private static int AdvanceSusan()
    {
        // If you haven't talked to Susan at all yet
        if (GameData.instance.susan == 0)
        {
            GameData.instance.susan = 1;
            return 0;
        }
        else if (GameData.instance.susan == 1)
        {
            GameData.instance.susan = 2;
            return 1;
        }
        return 2;
    }

    // 2: April
    private static int AdvanceApril()
    {
        if (GameData.instance.april1 == 0)
        {
            GameData.instance.april1 = 1;
            return 0;
        }
        return GameData.instance.april1;
    }

    // 3: Mom
    private static int AdvanceMom()
    {
        if (GameData.instance.mom1 == 0)
        {
            GameData.instance.mom1 = 1;
            return 0;
        }
        return GameData.instance.mom1;
    }
}
