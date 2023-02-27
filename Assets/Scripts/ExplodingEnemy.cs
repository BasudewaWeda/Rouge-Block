using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ExplodingEnemy : MonoBehaviour
{
    Transform target;
    public float speed;
    public float nextWaypointDist = 3;

    Path path;
    int currentWaypoint = 0;
    bool reachEnd;

    Seeker seeker;
    Rigidbody2D rb;

    public LayerMask masks;
    public LayerMask groundMask;
    public LayerMask explosionMask;

    public float health;
    public float enemyDamage;
    public float knockBackForce;
    public float damageConstant;
    public float healthConstant;
    public float attackRange;
    public float explodingRange;
    public float jumpForce;
    public float explosionRadius;
    public float explosionForce;

    public ParticleSystem deathEffect;
    public AudioClip deathSound;
    public AudioClip hurtSound;
    public AudioClip explosionSound;
    public AudioSource ac;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        speed = rb.mass * 400;
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, attackRange, masks);
        RaycastHit2D explodeRange = Physics2D.Raycast(transform.position, target.transform.position - transform.position, explodingRange, masks);
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundMask);

        if (hit.collider != null && hit.collider.CompareTag("Player") && groundCheck.collider != null && groundCheck.collider.CompareTag("Ground"))
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }

        if (explodeRange.collider != null && explodeRange.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerBullet"))
        {
            health -= collision.collider.gameObject.GetComponent<BulletScript>().damage;
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().cash += 25;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        ac.PlayOneShot(deathSound);
        Destroy(gameObject);
    }

    void Explode()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionMask);

        foreach(Collider2D obj in colliders)
        {
            Vector2 dir = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(dir * explosionForce, ForceMode2D.Force);

            obj.GetComponent<PlayerStats>().playerHealth -= enemyDamage;
        }

        ac.PlayOneShot(explosionSound);

        Destroy(gameObject);
    }
}
