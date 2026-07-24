using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Environment.Functionality
{
    [RequireComponent(typeof(Collider))]
    public class KillBox : MonoBehaviour, IPausable
    {
        private bool isPaused = false;
        public void Pause() => isPaused = true;
        public void Continue() => isPaused = false;

        void OnTriggerEnter(Collider _)
        {
            if (isPaused) return;
            OnContact();
        }
        void OnCollisionEnter(Collision _)
        {
            if (isPaused) return;
            OnContact();
        }

        void OnContact()
        {
            PlayerEvents.InvokePlayerDeath();
        }
    }
}