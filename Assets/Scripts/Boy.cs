using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : MonoBehaviour
{
    public int boyNumber;

    void Start()
    {
        if (boyNumber == 1)
        {
            // If you've already seen boy's first dialogue, he no longer appears in the spot
            if (GameData.instance.boy1 == 1)
            {
                Destroy(GameObject.Find("Boy"));
            }
        }
    }
}
