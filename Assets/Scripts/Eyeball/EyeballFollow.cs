using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballFollow : MonoBehaviour
{
    public Transform eyePupil;
    private Transform playerTransform;

    private float distance = .15f;

    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    void Start()
    {
        
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
