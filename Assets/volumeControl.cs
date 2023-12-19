using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] Slider volumeSlider;

    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            backgroundMusic.volume = 0.3f;
            volumeSlider.value = 0.3f;
        }
        else
        {
            backgroundMusic.volume = PlayerPrefs.GetFloat("musicVolume");
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    private void ChangeVolume()
    {
        backgroundMusic.volume = volumeSlider.value;
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }
}
