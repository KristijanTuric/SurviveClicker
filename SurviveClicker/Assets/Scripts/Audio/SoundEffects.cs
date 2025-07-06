using System.Collections;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip backgroundMusic;

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }

    public void PlayGameOver()
    {
        backgroundMusicSource.Pause();
        audioSource.PlayOneShot(gameOverSound);
    }

    public void RestartMusic()
    {
        backgroundMusicSource.Play();
    }
}
