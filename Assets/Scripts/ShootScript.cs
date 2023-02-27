using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire = true;
    public float timer;
    public float timeBetweenShot;
    public float fireRate;
    public float rotZ;

    public AudioSource audioSource;
    ParticleSystem muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        muzzleFlash = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime * fireRate;
            if (timer > timeBetweenShot)
            {
                timer = 0;
                canFire = true;
            }
        }

        if (Input.GetMouseButton(0) && canFire && !UIScript.isPaused)
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, transform.rotation);
            muzzleFlash.Play(muzzleFlash);
            audioSource.Play();
        }
    }
}
