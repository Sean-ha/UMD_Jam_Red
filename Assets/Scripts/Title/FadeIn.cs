using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public bool isTitle;
    public Button button;

    private PlayerController pc;

    void Start()
    {
        if (!isTitle)
        {
            pc = FindObjectOfType<PlayerController>();
            pc.SetCanMove(false);
            GetComponent<Image>().color = new Color(0, 0, 0, 1);
            LeanTween.alpha(GetComponent<RectTransform>(), 0, 2).setOnComplete(EnableMovement);
        }
        else
        {
            button.interactable = false;
            GetComponent<Image>().color = new Color(0, 0, 0, 1);
            LeanTween.alpha(GetComponent<RectTransform>(), 0, 2).setOnComplete(EnableButtons);
        }
    }

    private void EnableMovement()
    {
        pc.SetCanMove(true);
    }

    private void EnableButtons()
    {
        button.interactable = true;
    }
}
