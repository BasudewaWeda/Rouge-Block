using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float damage;
    public float force;
    private float destroyLimit = 3;
    private float destroyTime;
    public AudioSource ac;
    public AudioClip hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().damage;
        ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime += Time.deltaTime;
        if(destroyTime >= destroyLimit)
        {
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            DestroyBullet();
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            DestroyBullet();
            ac.PlayOneShot(hurtSound);
        }
    }
}
