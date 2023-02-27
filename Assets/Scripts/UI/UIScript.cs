using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class UIScript : MonoBehaviour
{
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI healthRegenText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI moveSpeedText;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI bestWaveText;

    public TextMeshProUGUI waveSurvivedText;
    public GameObject deathPanel;

    public Slider healthBar;
    public Slider regenBar;
    public Slider waveBar;

    public GameObject pausePanel;
    public static bool isPaused;

    AudioSource ac;
    public AudioClip buttonHoverSound;
    public AudioClip buttonPressSound;
    public AudioMixer audioMixer;
    public AudioMixer musicMixer;
    public Slider volumeSlider;
    public Slider musicSlider;

    Image healthBarImage;

    float bestWave;
    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AudioSource>();
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        musicSlider.value = PlayerPrefs.GetFloat("music");
    }

    // Update is called once per frame
    void Update()
    {
        bestWave = PlayerPrefs.GetFloat("highscore");

        cashText.text = "$" + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().cash.ToString();
        damageText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().damage.ToString();
        attackSpeedText.text = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject.GetComponent<ShootScript>().fireRate.ToString();
        healthRegenText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().playerHealthRegen.ToString();
        maxHealthText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().playerMaxHealth.ToString();
        moveSpeedText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().moveSpeed.ToString("0");

        waveText.text = "Wave " + GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().wave.ToString();
        bestWaveText.text = "Best : " + bestWave.ToString();
        waveSurvivedText.text = "Wave Survived  : " + (GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().wave - 1).ToString();

        healthBar.maxValue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().playerMaxHealth;
        healthBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().playerHealth;
        regenBar.maxValue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().regenRate;
        regenBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().regenTimer;
        waveBar.maxValue = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().timeBetweenWaves;
        waveBar.value  = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().waveTimer;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().dead)
        {
            deathPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }       
    }

    public void RetryScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ButtonPress();
    }

    public void MainMenuClick()
    {
        SceneManager.LoadScene(0);
        ResumeGame();
        ButtonPress();
    }

    public void ButtonPress()
    {
        ac.PlayOneShot(buttonPressSound);
    }

    public void ButtonHover()
    {
        ac.PlayOneShot(buttonHoverSound);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        ButtonPress();
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        ButtonPress();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetMusic(float volume)
    {
        musicMixer.SetFloat("music", volume);
        PlayerPrefs.SetFloat("music", volume);
    }
}
