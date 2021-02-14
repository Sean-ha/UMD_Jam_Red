using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    private int sceneToEnter;
    // Their position upon entering the new scene
    private Vector2 position;
    // Should player face left when entering new scene?
    private bool isFacingLeft;

    private PlayerController pc;

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
        pc = FindObjectOfType<PlayerController>();
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
}
