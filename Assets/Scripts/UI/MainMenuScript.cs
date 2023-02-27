using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    AudioSource ac;
    public AudioClip buttonHover;
    public AudioClip buttonPress;

    public TextMeshProUGUI bestScoreText;

    public AudioMixer audioMixer;
    public AudioMixer musicMixer;
    public Slider volumeSlider;
    public Slider musicSlider;

    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AudioSource>();

        bestScoreText.text = "Best : " + PlayerPrefs.GetFloat("highscore").ToString();

        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        musicSlider.value = PlayerPrefs.GetFloat("music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        ButtonClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads next scene
    }

    public void Quit()
    {
        ButtonClick();
        Application.Quit(); // Quits game
    }

    public void ButtonHover()
    {
        ac.PlayOneShot(buttonHover);
    }

    public void ButtonClick()
    {
        ac.PlayOneShot(buttonPress);
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
