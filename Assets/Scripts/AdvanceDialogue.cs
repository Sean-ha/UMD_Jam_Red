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
            case 4: return AdvanceMysteryMan();
            case 5: return AdvanceFarmer();
            case 6: return AdvanceKid();
            case 7: return AdvanceBoy1();
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

    // 4: Mystery Man
    private static int AdvanceMysteryMan()
    {
        if (GameData.instance.mysteryMan1 == 0)
        {
            GameData.instance.mysteryMan1 = 1;
            return 0;
        }
        return GameData.instance.mysteryMan1;
    }

    // 5: Farmer
    private static int AdvanceFarmer()
    {
        if (GameData.instance.flowerList[0])
        {
            GameData.instance.farmer = 3;
            return 3;
        }
        // The AutoNPC interaction
        if (!GameData.instance.gotWaterCan)
        {
            GameData.instance.gotWaterCan = true;
            GameData.instance.farmer = 1;
            return 0;
        }
        // Any manual interaction with farmer NPC
        if (GameData.instance.farmer == 1)
        {
            GameData.instance.farmer = 2;
            return 1;
        }
        return GameData.instance.farmer;
    }

    // 6: Kid
    private static int AdvanceKid()
    {
        if (GameData.instance.kid == 3)
        {
            return 3;
        }
        if (GameData.instance.flowerList[1])
        {
            GameData.instance.kid = 3;
            return 2;
        }
        if (GameData.instance.kid == 0)
        {
            GameData.instance.kid = 1;
            return 0;
        }
        return GameData.instance.kid;
    }

    // 7: BoyNormal
    private static int AdvanceBoy1()
    {
        if (GameData.instance.boy1 == 0)
        {
            GameData.instance.boy1 = 1;
            return 0;
        }
        return GameData.instance.boy1;
    }
}
