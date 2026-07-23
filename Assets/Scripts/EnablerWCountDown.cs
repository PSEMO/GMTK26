using PSEMO.Events;
using UnityEngine;
using System.Collections.Generic;

public class EnablerWCountDown : MonoBehaviour
{
    [SerializeField] List<GameObject> ActiveAtUp;
    [SerializeField] List<GameObject> ActiveAtNonUp;

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
        foreach (GameObject obj in ActiveAtUp)
        {
            obj.SetActive(isUp);
        }

        foreach (GameObject obj in ActiveAtNonUp)
        {
            obj.SetActive(!isUp);
        }
    }
}