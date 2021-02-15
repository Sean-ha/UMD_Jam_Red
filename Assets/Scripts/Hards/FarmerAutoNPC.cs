using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAutoNPC : MonoBehaviour
{
    private void Update()
    {
        if (GameData.instance.gotWaterCan)
        {
            Destroy(gameObject);
        }
    }
}
