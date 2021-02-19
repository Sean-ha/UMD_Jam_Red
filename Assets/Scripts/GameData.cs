using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    // Index is flowerId. Bool is if flower is grown or not
    public bool[] flowerList;
    // Gives most recent watered flowerId
    public int lastFlower;
    // Position and scene index of the last flower
    public Vector2 respawnPosition;
    public int respawnScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            flowerList = new bool[50];
             gotWaterCan = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [HideInInspector]
    public int susan;
    public int april1;
    public int mom1;
    public int mysteryMan1;
    public bool gotWaterCan;
    public int farmer;
    public int kid;
    public int boy1;
    public bool boy2;
    public int deadGuy;
}
