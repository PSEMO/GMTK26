using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using PSEMO.Events;

public class CS_AmbianceManager : MonoBehaviour
{
    public static CS_AmbianceManager Instance { get; private set; }

    [Header("Ambiance globale")]
    [SerializeField] private EventReference ambienceEvent;

    [Header("Countdown")]
    [SerializeField] private EventReference countdownEndEvent;
    [SerializeField] private EventReference countdownBeepEvent;

    private EventInstance ambienceInstance;
    private EventInstance musicInstance;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ambienceInstance = RuntimeManager.CreateInstance(ambienceEvent);
        ambienceInstance.start();
    }

    void OnEnable()
    {
        CountdownEvent.OnCountDown += OnCountDown;
        CountdownEvent.OnCountDownBeep += OnCountDownBeep;
    }

    void OnDisable()
    {
        CountdownEvent.OnCountDown -= OnCountDown;
        CountdownEvent.OnCountDownBeep -= OnCountDownBeep;
    }

    void OnCountDown(bool isUp)
    {
        RuntimeManager.PlayOneShot(countdownEndEvent, transform.position);
    }

    void OnCountDownBeep()
    {
        RuntimeManager.PlayOneShot(countdownBeepEvent, transform.position);
    }

    public void PlayMusic(EventReference musicEvent)
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }
}