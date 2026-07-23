using System.Collections;
using UnityEngine;

namespace PSEMO.Environment.Functionality
{
    [RequireComponent(typeof(Collider))]
    public class Disappear : MonoBehaviour
    {
        [SerializeField] private GameObject ObjectToDisappear;

        [Tooltip("Time in seconds before the platform disappears after being stepped on.")]
        [SerializeField] private float timeToDisappear = 1.0f;
        
        [Tooltip("Time in seconds before the platform respawns. Set to 0 if it should never respawn.")]
        [SerializeField] private float respawnTime = 3.0f;

        private bool isSteppedOn = false;
        private MeshRenderer[] meshRenderers;
        private Collider platformCollider;

        private void Awake()
        {
            meshRenderers = ObjectToDisappear.GetComponentsInChildren<MeshRenderer>();
            platformCollider = ObjectToDisappear.GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision _)
        {
            CheckContact();
        }

        private void OnTriggerEnter(Collider _)
        {
            CheckContact();
        }

        private void CheckContact()
        {
            if (isSteppedOn) return;

            StartCoroutine(DisappearRoutine());
        }

        private IEnumerator DisappearRoutine()
        {
            isSteppedOn = true;

            yield return new WaitForSeconds(timeToDisappear);

            SetVisualsAndCollision(false);

            if (respawnTime > 0)
            {
                yield return new WaitForSeconds(respawnTime);

                SetVisualsAndCollision(true);
                isSteppedOn = false;
            }
        }

        private void SetVisualsAndCollision(bool state)
        {
            if (platformCollider != null)
            {
                platformCollider.enabled = state;
            }

            foreach (var mr in meshRenderers)
            {
                if (mr != null)
                {
                    mr.enabled = state;
                }
            }
        }
    }
}
