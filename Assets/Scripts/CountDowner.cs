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
    int lastSecond; // pour détecter le changement de seconde

    void Start()
    {
        maxTimer = timer;
        currentUpState = initialUpState;
        lastSecond = Mathf.CeilToInt(timer);

        CountdownEvent.InvokeOnCountDown(currentUpState);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Détection du passage à la seconde inférieure → beep
        int currentSecond = Mathf.CeilToInt(timer);
        if (currentSecond < lastSecond && currentSecond > 0)
        {
            lastSecond = currentSecond;
            CountdownEvent.InvokeOnCountDownBeep(); // nouveau event
        }

        if (timer < 0)
        {
            currentUpState = !currentUpState;
            timer += maxTimer;
            lastSecond = Mathf.CeilToInt(timer);

            CountdownEvent.InvokeOnCountDown(currentUpState);
        }

        txt.text = timer.ToString("F1");
    }
}