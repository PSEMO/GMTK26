using UnityEngine;
using FMODUnity;

public class CS_LVLMusic : MonoBehaviour
{
    [Header("LVL Music")]
    [SerializeField] private EventReference musicEvent;

    void Start()
    {
        if (CS_AmbianceManager.Instance != null)
            CS_AmbianceManager.Instance.PlayMusic(musicEvent);
        else
            Debug.LogWarning("CS_AmbianceManager don't exist");
    }
}