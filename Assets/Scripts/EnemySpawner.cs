using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyArray;
    public float timeBetweenSpawns;
    public float spawnTimer;

    public int wave;
    public int enemyIndex;
    public float waveTimer;
    public float timeBetweenWaves;

    Transform spawnLocation;
    public LayerMask groundLayer;

    float offsetX;
    float offsetY;
    Vector3 fixedSpawnLocation;
    bool takenPosition = false;

    AudioSource ac;
    public AudioClip waveUpSound;
    public ParticleSystem enemySpawnEffect;
    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AudioSource>();
        spawnLocation = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnTimer = timeBetweenSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;

        if (spawnTimer >= timeBetweenSpawns) // Spawns random enemy when spawn timer is equal to time between spawns
        {
            enemyIndex = Random.Range(0, enemyArray.Length);
            offsetX = Random.Range(-20, 20);
            if (!takenPosition)
            {
                fixedSpawnLocation = spawnLocation.position;
                takenPosition = true;
            }

            if(enemyIndex == 1)
            {
                offsetY = Random.Range(5, 15);
                EnemySpawnEffect(new Vector3(offsetX, offsetY, 0), fixedSpawnLocation);
                StartCoroutine(EnemySpawn(enemyIndex, fixedSpawnLocation, new Vector3(offsetX, offsetY, 0), 0.5f)); // Spawning flying enemy
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(fixedSpawnLocation + new Vector3(offsetX, -1f, 0), -Vector2.up, groundLayer); // Calculating offset to make sure enemy spawns on ground
                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    offsetY = hit.point.y - fixedSpawnLocation.y;
                    Debug.DrawRay(fixedSpawnLocation + new Vector3(offsetX, -1f, 0), -Vector2.up, Color.red, 5f);
                }
                EnemySpawnEffect(new Vector3(offsetX, offsetY, 0), fixedSpawnLocation);
                StartCoroutine(EnemySpawn(enemyIndex, fixedSpawnLocation,  new Vector3(offsetX, offsetY, 0), 0.5f)); // Spawning ground enemy
            }
            spawnTimer = 0;
        }

        if (waveTimer >= timeBetweenWaves) // Increase wave count
        {
            wave += 1;
            GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopScript>().price = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopScript>().priceConstant * wave; //Increase shop price 
            waveTimer = 0;
            ac.PlayOneShot(waveUpSound);
            if(PlayerPrefs.GetFloat("highscore") < wave - 1)
            {
                PlayerPrefs.SetFloat("highscore", wave - 1); // saving highscore
            }
        }
    }

    IEnumerator EnemySpawn(int enemyIndex, Vector3 spawnLoc, Vector3 offset, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(enemyArray[enemyIndex], spawnLoc + offset, Quaternion.identity); // Function to spawn enemy
        takenPosition = false;
    }

    void EnemySpawnEffect(Vector3 offset, Vector3 spawnLoc)
    {
        Instantiate(enemySpawnEffect, spawnLoc + offset, Quaternion.identity); // Function to spawn enemy effect
    }
}
