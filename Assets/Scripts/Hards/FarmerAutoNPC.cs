using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAutoNPC : MonoBehaviour
{
    public DialogueObject needToVisitHospitalDialogue;

    private void Start()
    {
        // You can't go past if you haven't visited your mom in the hospital yet
        if (GameData.instance.mom1 == 0)
        {
            DialogueTrigger dt = GetComponent<DialogueTrigger>();
            dt.npcId = -1;
            dt.dialogueObjects[0] = needToVisitHospitalDialogue;
        }
    }

    private void Update()
    {
        if (GameData.instance.gotWaterCan)
        {
            Destroy(gameObject);
        }
    }
}
