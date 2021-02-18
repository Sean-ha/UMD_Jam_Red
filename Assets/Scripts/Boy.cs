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
            if (GameData.instance.boy1 == 1 || GameData.instance.boy2)
            {
                Destroy(gameObject);
            }
        }
        else if (boyNumber == 2)
        {
            if (GameData.instance.boy2)
            {
                Destroy(gameObject);
            }
        }
    }
}
