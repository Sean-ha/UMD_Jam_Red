using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSpeed : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().speed = Random.Range(0.8f, 1.2f);
    }
}
