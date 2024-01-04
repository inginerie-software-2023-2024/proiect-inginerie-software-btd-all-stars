using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXControlVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }

    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume();
    }
}
