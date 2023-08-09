using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject[] buffArray;
    int n;
    float cash;
    public float price;
    public float priceConstant = 25;
    public AudioSource ac;
    public AudioClip buySound;


    public void Update()
    {
        
    }
    public void Purchase()
    {
        Debug.Log("Interacted");
        cash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().cash;

        if(cash >= price)
        {
            n = Random.Range(0, buffArray.Length);
            Instantiate(buffArray[n], transform.position + new Vector3(0, 1, 0), transform.rotation);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().cash -= price;
            ac.PlayOneShot(buySound);
        }
    }
}
