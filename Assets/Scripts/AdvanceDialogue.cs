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
}
