using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().playerHealth = 0; // Kill player if the player touches the void
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
