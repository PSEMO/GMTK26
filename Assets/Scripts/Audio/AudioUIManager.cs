using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class AudioUIManager : MonoBehaviour
{
    public static AudioUIManager Instance { get; private set; }

    [Header("Sons UI")]
    [SerializeField] private EventReference clickEvent;
    [SerializeField] private EventReference hoverEvent;

    [Header("Sliders Settings")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        if (masterSlider != null)
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void PlayClick() => RuntimeManager.PlayOneShot(clickEvent);
    public void PlayHover() => RuntimeManager.PlayOneShot(hoverEvent);

    void SetMasterVolume(float value)
    {
        masterBus.setVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void SetMusicVolume(float value)
    {
        musicBus.setVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    void SetSFXVolume(float value)
    {
        sfxBus.setVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}