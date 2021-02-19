using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballFollow : MonoBehaviour
{
    public Transform eyePupil;
    public bool isLeft;

    private Transform playerTransform;

    private float distance = .3f;

    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    private void Start()
    {
        // Moves eyes into proper location based on where player enters the room
        if (isLeft)
        {
            // Enter from right side
            if (playerTransform.localPosition.x > 40)
            {
                transform.localPosition = new Vector2(65, 2.88f);
            }
            // Enter from left side
            else
            {
                transform.localPosition = new Vector2(16f, 2.88f);
            }

            GameObject.Find("EyeballRight").transform.localPosition = 
                new Vector2(transform.localPosition.x + 3.5f, transform.localPosition.y);
        }
    }

    void Update()
    {
        // Get angle from eye to player
        float x = transform.localPosition.x - playerTransform.localPosition.x;
        float y = transform.localPosition.y - playerTransform.localPosition.y;
        float angle = Mathf.Atan2(y, x);

        // Set position of eye
        eyePupil.localPosition = new Vector2(transform.localPosition.x - Mathf.Cos(angle) * distance,
            transform.localPosition.y - Mathf.Sin(angle) * distance);

        // eyePupil.localPosition
    }
}
