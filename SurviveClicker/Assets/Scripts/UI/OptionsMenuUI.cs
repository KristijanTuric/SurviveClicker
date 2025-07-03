using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [Header("Menu Panels")] 
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Settings")] 
    [SerializeField] private AudioSource audioSource;

    [Header("Text Elements")] 
    [SerializeField] private TMP_Text volumeText;

    [Header("Sliders")] 
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(AudioVolumeChanged);
        AudioVolumeChanged(volumeSlider.value);
    }

    public void ReturnToMainMenu()
    {
        optionsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void AudioVolumeChanged(float value)
    {
        volumeText.text = $"{Mathf.RoundToInt(value * 100)}%";
        audioSource.volume = value;
    }
}
