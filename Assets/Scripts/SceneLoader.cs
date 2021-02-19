using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    private int sceneToEnter;
    // Their position upon entering the new scene
    private Vector2 position;
    // Should player face left when entering new scene?
    private bool isFacingLeft;

    private PlayerController pc;
    private Image blackTransition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneChange;        
    }

    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        // Black screen transition
        blackTransition = GameObject.FindGameObjectWithTag("BlackTransition").GetComponent<Image>();
        blackTransition.color = new Color(0, 0, 0, 1);
        blackTransition.rectTransform.anchoredPosition = new Vector2(0, 0);

        LeanTween.move(blackTransition.rectTransform, new Vector2(-2000, 0), .15f).setOnComplete(OnBlackTransitionComplete);

        pc = FindObjectOfType<PlayerController>();
        if (pc != null)
        {
            pc.SetCanMove(false);
        }

        // Move player to the proper location
        if (position.x != 0 && position.y != 0)
        {
            pc.transform.localPosition = position;
        }
        if (isFacingLeft)
        {
            pc.SetFacingLeft();
        }
    }

    public void SetSceneLoadData(int sceneNum, Vector2 pos, bool faceLeft)
    {
        sceneToEnter = sceneNum;
        position = pos;
        isFacingLeft = faceLeft;
    }

    private void OnBlackTransitionComplete()
    {
        if (pc != null)
        {
            pc.SetCanMove(true);
        }
        blackTransition.color = new Color(0, 0, 0, 0);
    }
}
