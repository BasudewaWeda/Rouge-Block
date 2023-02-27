using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupCollider : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    public AudioSource ac;
    public AudioClip powerUpSound;
    public ParticleSystem powerEffect;

    private void Start()
    {
        ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) // Removes powerup after getting picked up by player
        {
            Instantiate(powerEffect, collision.collider.transform.position, Quaternion.identity);
            ac.PlayOneShot(powerUpSound);
            powerupEffect.Apply(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
