using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAudio : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        source.volume = 0;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float interval = 0.005f;
        float max = 0.6f;

        while (source.volume < max)
        {
            source.volume += interval;
            yield return null;
        }
    }
}
