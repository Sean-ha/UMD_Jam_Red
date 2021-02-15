using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public int sceneToEnter;
    // Their position upon entering the new scene
    public Vector2 position;
    // Should player face left when entering new scene?
    public bool isFacingLeft;

    private Image blackTransition;
    private SoundManager sm;

    private void Awake()
    {
        blackTransition = GameObject.FindGameObjectWithTag("BlackTransition").GetComponent<Image>();
    }

    private void Start()
    {
        sm = SoundManager.instance;
    }

    public void EnterDoor()
    {
        sm.PlaySound(SoundManager.Sound.Door);

        blackTransition.color = new Color(0, 0, 0, 1);
        blackTransition.rectTransform.anchoredPosition = new Vector2(2000, 0);

        LeanTween.move(blackTransition.rectTransform, new Vector2(0, 0), .15f).setOnComplete(TransitionScene);
    }

    private void TransitionScene()
    {
        SceneLoader.instance.SetSceneLoadData(sceneToEnter, position, isFacingLeft);
        SceneManager.LoadScene(sceneToEnter);
    }
}
