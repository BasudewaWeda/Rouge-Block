using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private Vector3 targetPos;
    private Transform target;
    private Rigidbody2D rb;
    public float damage;
    public float damageConstant;
    public float force;
    private float destroyLimit = 3;
    private float destroyTime;
    public AudioSource ac;
    public AudioClip hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        targetPos = target.position;
        Vector3 direction = targetPos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        damage = damageConstant * GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().wave;
        ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime += Time.deltaTime;
        if (destroyTime >= destroyLimit)
        {
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            DestroyBullet();
        }

        if (collision.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player.canBeHit)
            {
                player.playerHealth -= damage;
                player.canBeHit = false;
                ac.PlayOneShot(hurtSound);
            }
            DestroyBullet();
        }
    }
}
