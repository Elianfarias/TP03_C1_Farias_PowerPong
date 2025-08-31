using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    [SerializeField] private AudioSource soundEffectAudioSource;
    [SerializeField] private AudioSource BackgroundAudioSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        soundEffectAudioSource.clip = audioClip;
        soundEffectAudioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        BackgroundAudioSource.Stop();
    }
}
