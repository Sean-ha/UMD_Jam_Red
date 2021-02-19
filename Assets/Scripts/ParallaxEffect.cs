using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] backgrounds;
    public float parallaxScale;

    private Transform cam;
    private Vector3 previousCamPosition;

    void Awake()
    {
        // Set up camera
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPosition = cam.position;
    }

    void Update()
    {
        if (cam.position.x != previousCamPosition.x)
        {
            
            for (int i = 0; i < backgrounds.Length; i++)
            {
                float parallax = (previousCamPosition.x - cam.position.x) * parallaxScale / 50;
                float backgroundTargetX = backgrounds[i].position.x + parallax;

                Vector3 backgroundTargetPosition = new Vector3(backgroundTargetX, backgrounds[i].position.y,
                    backgrounds[i].position.z);

                backgrounds[i].position = backgroundTargetPosition;
            }
            previousCamPosition = cam.position;
        }
    }
}