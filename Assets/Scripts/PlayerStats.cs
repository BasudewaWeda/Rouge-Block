using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Rigidbody2D rb;
    public float playerHealth = 100;
    public float playerMaxHealth = 100;
    public float playerHealthRegen = 5;
    public float regenRate = 2;
    public float regenTimer;
    public float invincibilityTime = 1.5f;
    public float invincibilityTimer;
    public bool canBeHit = true;
    public bool dead = false;

    public float damage = 10;

    public float cash = 0;

    public AudioSource ac;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    public ParticleSystem deathEffect;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per framT
    void Update()
    {

        if (playerHealth <= 0 && !dead)
        {
            Death();
            dead = true;
        }

        if (!canBeHit)
        {
            invincibilityTimer += Time.deltaTime;
        }

        if (invincibilityTimer >= invincibilityTime)
        {
            canBeHit = true;
            invincibilityTimer = 0;
        }

        regenTimer += Time.deltaTime;

        if ((playerHealth < playerMaxHealth) && (regenTimer >= regenRate))
        {
            playerHealth += playerHealthRegen;
            regenTimer = 0;
        }

        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }

    }

    void Death()
    {
        ac.PlayOneShot(deathSound);
        Invoke("RemovePlayer", 0.05f);
    }
    
    void RemovePlayer()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
