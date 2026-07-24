using PSEMO.Environment.Functionality;
using UnityEngine;
using PSEMO.Core.Persistence;

namespace PSEMO.Environment.Movement
{
    public class Rotater : MonoBehaviour, IPoolable, IPersistable, IPausable
    {
        private bool isPaused = false;
        private Vector3 storedAngularVelocity;
        private bool wasGravityEnabled;

        public void Pause()
        {
            isPaused = true;
            if (rb != null)
            {
                storedAngularVelocity = rb.angularVelocity;
                rb.angularVelocity = Vector3.zero;
                wasGravityEnabled = rb.useGravity;
                rb.useGravity = false;
            }
        }
        public void Continue()
        {
            isPaused = false;
            if (rb != null)
            {
                rb.angularVelocity = storedAngularVelocity;
                rb.useGravity = wasGravityEnabled;
            }
        }

        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private Vector3 rotationAxis = Vector3.forward;
        [SerializeField] private bool unscaledTime = false;
        [SerializeField] private bool useGlobalTime = false;

        private Quaternion initialRotation;
        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            initialRotation = transform.rotation;
        }

        void Start()
        {
            
        }

        private void Update()
        {
            if (isPaused) return;
            if (rb != null) return;

            if (useGlobalTime)
            {
                float time = unscaledTime ? Time.unscaledTime : Time.time;
                transform.rotation = initialRotation * Quaternion.AngleAxis(rotationSpeed * time, rotationAxis);
            }
            else
            {
                if (unscaledTime)
                {
                    transform.Rotate(rotationAxis, rotationSpeed * Time.unscaledDeltaTime);
                }
                else
                {
                    transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            if (isPaused) return;
            if (rb == null) return;

            if (useGlobalTime)
            {
                float time = unscaledTime ? Time.unscaledTime : Time.time;
                rb.MoveRotation(initialRotation * Quaternion.AngleAxis(rotationSpeed * time, rotationAxis));
            }
            else
            {
                float dt = unscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
                rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(rotationSpeed * dt, rotationAxis));
            }
        }

        public void ResetObject()
        {
            transform.rotation = initialRotation;
            if (rb != null)
            {
                rb.rotation = initialRotation;
                rb.angularVelocity = Vector3.zero;
            }
        }

        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            RotaterSaveData data = JsonUtility.FromJson<RotaterSaveData>(jsonData);
            
            transform.rotation = data.rotation;
            if (rb != null)
            {
                rb.rotation = data.rotation;
            }
        }

        public string SaveData()
        {
            RotaterSaveData data = new()
            {
                rotation = transform.rotation
            };
            return JsonUtility.ToJson(data);
        }
        //=========================
    }
}