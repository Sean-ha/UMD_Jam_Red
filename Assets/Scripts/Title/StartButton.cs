using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Image blackScreen;

    public void OnPress()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        GetComponent<Button>().interactable = false;
        LeanTween.alpha(blackScreen.rectTransform, 1, 1.5f).setOnComplete(GoToGame);
    }

    private void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
