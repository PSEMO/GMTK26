using TMPro;
using PSEMO.Events;
using UnityEngine;

public class CountDowner : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] float timer = 3f;
    [SerializeField] bool initialUpState = true;

    bool currentUpState;
    float maxTimer;

    void Start()
    {
        maxTimer = timer;
        currentUpState = initialUpState;

        CountdownEvent.InvokeOnCountDown(currentUpState);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            currentUpState = !currentUpState;
            timer += maxTimer;

            CountdownEvent.InvokeOnCountDown(currentUpState);
        }

        txt.text = timer.ToString("F1");
    }
}