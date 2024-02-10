using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem> {
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundsSource;
    [SerializeField] private AudioMixer audioMixer;
    private void Start()
    {
        LoadSoundOptions();
    }

    private void LoadSoundOptions()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            float volume = PlayerPrefs.GetFloat("music");
            audioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        }

        if (PlayerPrefs.HasKey("sfx"))
        {
            float volume = PlayerPrefs.GetFloat("sfx");
            audioMixer.SetFloat("sfx", Mathf.Log10(volume)*20);
        }
    }

    public void PlayMusic(AudioClip clip) {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1) {
        _soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1) {
        _soundsSource.PlayOneShot(clip, vol);
    }
}