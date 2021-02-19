using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballCollider : MonoBehaviour
{
    private GameObject eyeBallLeft;
    private GameObject eyeBallRight;

    private static bool isOpen;

    private void Awake()
    {
        eyeBallLeft = GameObject.Find("EyeballLeft");
        eyeBallRight = GameObject.Find("EyeballRight");
    }

    private void Start()
    {
        isOpen = false;
    }

    private void OpenEyes()
    {
        isOpen = true;
        SoundManager.instance.PlaySound(SoundManager.Sound.EyesOpen);
        eyeBallLeft.GetComponent<Animator>().Play("Eyeball_Open");
        eyeBallRight.GetComponent<Animator>().Play("Eyeball_Open");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isOpen)
            {
                OpenEyes();
            }
        }
    }
}
