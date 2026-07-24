using UnityEngine;
using PSEMO.Environment.Functionality;

namespace PSEMO.Environment.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Faller : MonoBehaviour, IMover, IPoolable, IPausable
    {
        private bool isPaused = false;
        private Vector3 storedVelocity;
        private bool wasGravityEnabled;

        public void Pause()
        {
            isPaused = true;
            if (rb != null)
            {
                storedVelocity = rb.linearVelocity;
                rb.linearVelocity = Vector3.zero;
                wasGravityEnabled = rb.useGravity;
                rb.useGravity = false;
            }
        }
        public void Continue()
        {
            isPaused = false;
            if (rb != null)
            {
                rb.linearVelocity = storedVelocity;
                rb.useGravity = wasGravityEnabled;
            }
        }

        [SerializeField] private float maxSpeed = 20f;
        [SerializeField] private float gravity = 9.8f;
    
        private float currentSpeed = 0f;
        private Vector3 directionalSpeed = Vector3.zero;

        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (isPaused) return;

            if (currentSpeed >= maxSpeed)
            {
                directionalSpeed = Vector3.down * maxSpeed;
            }
            else
            {
                currentSpeed += gravity * Time.fixedDeltaTime;
                directionalSpeed = Vector3.down * currentSpeed;
            }
            rb.linearVelocity = directionalSpeed;
        }

        public Vector3 GetVelocity()
        {
            return directionalSpeed;
        }

        public void ResetObject()
        {
            Continue();
            currentSpeed = 0f;
            directionalSpeed = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}