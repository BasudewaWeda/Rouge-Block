using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpad : MonoBehaviour
{
    public float jumpForce;

    AudioSource ac;
    public AudioClip jumpSound;
    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }

        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyFly"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpForce * Vector2.up, ForceMode2D.Force);
        }

        ac.PlayOneShot(jumpSound);
    }
}
