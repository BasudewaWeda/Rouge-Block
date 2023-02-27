using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    Transform target;
    public GameObject bullet;
    public LayerMask mask;
    public float fireRate = 5;
    public float timer;
    public float timeBetweenShot = 2;
    public float attackRange;

    bool canFire = true;
    bool inRange;

    public AudioSource ac;
    public AudioClip shootSound;
    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canFire)
        {
            timer += Time.deltaTime * fireRate;
            if (timer > timeBetweenShot)
            {
                timer = 0;
                canFire = true;
            }
        }

        if (canFire && inRange)
        {
            canFire = false;
            Instantiate(bullet, transform.position, transform.rotation);
            ac.PlayOneShot(shootSound);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, attackRange, mask); // Raycast to check if enemy sees player

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
    }
}
