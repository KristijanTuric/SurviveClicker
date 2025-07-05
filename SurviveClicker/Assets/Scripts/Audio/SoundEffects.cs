using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
