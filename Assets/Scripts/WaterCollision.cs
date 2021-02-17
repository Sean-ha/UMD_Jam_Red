using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flower"))
        {
            collision.GetComponent<Flower>().GrowFlower();
        }
    }
}
