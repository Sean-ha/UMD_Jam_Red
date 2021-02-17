using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public int flowerId;
    

    private void Start()
    {
        if (GameData.instance.flowerList[flowerId])
        {
            GetComponent<Animator>().Play("Flower_Grown");
        }
    }

    public void GrowFlower()
    {
        // If flower isn't grown, then grow it
        if (!GameData.instance.flowerList[flowerId])
        {
            GetComponent<Animator>().Play("Flower_Grow");
            GameData.instance.flowerList[flowerId] = true;
            GameData.instance.lastFlower = flowerId;
            GameData.instance.respawnPosition = transform.position;
            GameData.instance.respawnScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }
    }

    public void PlayFlowerSound()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.FlowerBloom);
    }
}
