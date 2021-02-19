using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public CinemachineVirtualCamera vcamMain;
    public CinemachineVirtualCamera vcamEnd;
    public GameObject blackScreen;
    public TextMeshProUGUI endText;

    private PlayerController pc;
    private bool playerMoving;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (playerMoving)
        {
            if (pc.transform.localPosition.x > 80)
            {
                pc.SetFacingLeft();
                pc.SetHorizontal(-1);
            }
            else
            {
                pc.SetHorizontal(0);
                StartCoroutine(PlayerSit());
                playerMoving = false;
            }
        }
    }

    public void StartEnding()
    {
        playerMoving = true;

    }

    private void PlayIdleAnimation()
    {
        GetComponent<Animator>().Play("Boy_Idle");
    }

    private IEnumerator PlayerSit()
    {
        yield return new WaitForSeconds(1);
        pc.GetComponent<Animator>().Play("Player_TurnToBack");
        yield return new WaitForSeconds(1);
        SoundManager.instance.PlaySound(SoundManager.Sound.Sit);
        pc.GetComponent<Animator>().Play("Player_Sit");

        yield return new WaitForSeconds(2.4f);
        GetComponent<Animator>().Play("Boy_Turn");
        yield return new WaitForSeconds(2.4f);
        GetComponent<Animator>().Play("Boy_TurnBack");

        vcamMain.gameObject.SetActive(false);
        vcamEnd.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        LeanTween.moveLocalY(vcamEnd.gameObject, vcamEnd.transform.localPosition.y + 1.8f, 5);

        yield return new WaitForSeconds(10);

        blackScreen.gameObject.SetActive(true);
        LeanTween.alpha(blackScreen, 1, 3);
        yield return new WaitForSeconds(5.5f);

        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        endText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3);

        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        endText.text = "a game by sean";

        yield return new WaitForSeconds(3);

        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        endText.gameObject.SetActive(false);

        yield return new WaitForSeconds(3);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
