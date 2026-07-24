using UnityEngine;
using PSEMO.Events;
using System.Collections.Generic;

public class CountDownPauser : MonoBehaviour
{
    [SerializeField] bool isUpMeansActive = true;

    private IPausable[] pausables;

    private void Awake()
    {
        pausables = GetComponents<IPausable>();
    }

    private void OnEnable()
    {
        CountdownEvent.OnCountDown += HandleCountdown;
    }

    private void OnDisable()
    {
        CountdownEvent.OnCountDown -= HandleCountdown;
    }

    private void HandleCountdown(bool isUp)
    {
        if (pausables == null) return;
        
        foreach (var pausable in pausables)
        {
            if (isUp == isUpMeansActive)
            {
                pausable.Continue();
            }
            else
            {
                pausable.Pause();
            }
        }
    }
}
