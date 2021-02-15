using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryMan : MonoBehaviour
{
    void Start()
    {
        if (GameData.instance.mom1 == 0 || GameData.instance.mysteryMan1 == 1)
        {
            gameObject.SetActive(false);
        }   
    }
}
