using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private PlayerController pc;

    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        pc.SetCanMove(false);
        GetComponent<Image>().color = new Color(0, 0, 0, 1);
        LeanTween.alpha(GetComponent<RectTransform>(), 0, 2).setOnComplete(EnableMovement);
    }

    private void EnableMovement()
    {
        pc.SetCanMove(true);
    }
}
