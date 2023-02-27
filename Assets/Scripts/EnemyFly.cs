using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyFly : MonoBehaviour
{
    Transform target;
    public float speed;
    public float nextWaypointDist = 3;

    Path path;
    int currentWaypoint = 0;
    bool reachEnd;

    Seeker seeker;
    Rigidbody2D rb;

    public float health;
    public float enemyDamage;
    public float knockBackForce;
    public float damageConstant;
    public float healthConstant;

    public ParticleSystem deathEffect;
    public AudioClip deathSound;
    public AudioClip hurtSound;
    public AudioSource ac;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        speed = rb.mass * 200;
        enemyDamage = damageConstant * GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().wave;
        health = healthConstant * GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().wave;
        ac = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        Vector3 rotation = target.position - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }
        else if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEnd = true;
            return;
        }
        else
        {
            reachEnd = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; // Direction to move to
        Vector2 force = direction * speed * Time.deltaTime; // calculating force 

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]); // ditance between current pos and current waypoint
        if (distance < nextWaypointDist)
        {
            currentWaypoint++; // Move on to next waypoint 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerBullet"))
        {
            health -= collision.collider.gameObject.GetComponent<BulletScript>().damage;
        }

        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.GetComponent<PlayerStats>().canBeHit)
            {
                collision.collider.gameObject.GetComponent<PlayerStats>().playerHealth -= enemyDamage;

                Vector2 knockbackDir = (collision.collider.transform.position - transform.position).normalized; // Knockback direction
                Vector2 knockback = knockbackDir * knockBackForce;
                collision.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(knockback, ForceMode2D.Impulse);
                collision.collider.gameObject.GetComponent<PlayerStats>().canBeHit = false;
                ac.PlayOneShot(hurtSound);
            }
        }
    }

    void OnPathFound(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathFound); // Only start path when seeker is done
        }
    }

    void Death()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().cash += 15;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        ac.PlayOneShot(deathSound);
        Destroy(gameObject);
    }
}
