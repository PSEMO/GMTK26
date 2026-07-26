using System;

namespace PSEMO.Events
{
    public static class CountdownEvent
    {
        public static event Action<bool> OnCountDown;
        public static event Action OnCountDownBeep;

        public static void InvokeOnCountDown(bool isUp) => OnCountDown?.Invoke(isUp);
        public static void InvokeOnCountDownBeep() => OnCountDownBeep?.Invoke();
    }
}