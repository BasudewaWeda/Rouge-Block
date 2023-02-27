using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLaunch : MonoBehaviour
{
    public Rigidbody2D rb;
    public float force = 1;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Launch power up after getting instantiated 
        direction = new Vector3(Random.Range(-0.5f, 0.5f), 1, 0); 
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
