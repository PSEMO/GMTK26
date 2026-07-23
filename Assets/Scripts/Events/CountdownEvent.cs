using System;

namespace PSEMO.Events
{
    public static class CountdownEvent
    {
        public static event Action<bool> OnCountDown;
        public static void InvokeOnCountDown(bool isUp) => OnCountDown?.Invoke(isUp);
    }
}