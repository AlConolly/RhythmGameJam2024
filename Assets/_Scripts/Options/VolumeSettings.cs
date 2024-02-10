using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    
    // * These values correspond to the exposed values in the audioMixer
    const string SFX_VALUE = "sfx";
    const string MUSIC_VALUE = "music";

    private void Start()
    {
        if (PlayerPrefs.HasKey(MUSIC_VALUE))
        {
            LoadSettings();
        } else SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat(MUSIC_VALUE, Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat(MUSIC_VALUE, volume);
    }

    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat(SFX_VALUE, Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat(SFX_VALUE, volume);
    }

    /// <summary>
    /// This function will load playerprefs and apply them
    /// </summary>
    private void LoadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VALUE);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VALUE);

        SetMusicVolume();
        SetSfxVolume();
    }
}
