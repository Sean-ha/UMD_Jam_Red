using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [HideInInspector]
    public ParticleSystem waterParticles;

    private GameObject waterCollision;
    private PlayerController pc;

    private void Awake()
    {
        waterParticles = transform.Find("WaterParticles").GetComponent<ParticleSystem>();
        pc = transform.parent.GetComponent<PlayerController>();
        waterCollision = transform.parent.Find("WaterCollision").gameObject;
    }

    private void Start()
    {
        waterCollision.SetActive(false);
        gameObject.SetActive(false);
    }

    public void PlayWaterParticles()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Watering);
        waterParticles.Play();
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
        pc.SetCanMove(true);
    }

    public void EnableWaterCollision()
    {
        waterCollision.SetActive(true);
    }

    public void DisableWaterCollision()
    {
        waterCollision.SetActive(false);
    }
}
