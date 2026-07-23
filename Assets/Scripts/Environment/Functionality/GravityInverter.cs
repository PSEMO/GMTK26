using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Environment.Functionality
{
    public class GravityInverter : MonoBehaviour
    {
        [Tooltip("If true, isUp from countdown makes gravity point DOWN. Otherwise, isUp makes gravity point UP.")]
        [SerializeField] private bool isUpMeansDown = false;

        private float gravityMagnitude;

        private void Awake()
        {
            // Store the initial gravity magnitude so we can re-apply it properly
            gravityMagnitude = Mathf.Abs(Physics.gravity.y);
            if (gravityMagnitude == 0) gravityMagnitude = 9.81f; // Fallback
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
            // Determine if we should pull upwards based on the event and our logic toggle
            bool pullUp = isUpMeansDown ? !isUp : isUp;
            
            Physics.gravity = new Vector3(
                Physics.gravity.x, 
                pullUp ? gravityMagnitude : -gravityMagnitude, 
                Physics.gravity.z
            );
        }
    }
}
