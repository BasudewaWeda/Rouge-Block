using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    ParticleSystem effect;
    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Destroy", 0.5f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
