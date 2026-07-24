using UnityEngine;
using PSEMO.Core.Management;

namespace PSEMO.Environment.Functionality
{
    public class Spawner : MonoBehaviour, IPausable
    {
        private bool isPaused = false;
        public void Pause() => isPaused = true;
        public void Continue() => isPaused = false;

        [Tooltip("Time in seconds between each spawn.")]
        [SerializeField] private  float SpawnInterval = 2f;
    
        [Tooltip("Time in seconds before the first spawn.")]
        [SerializeField] private  float InitialDelay = 0f;

        [Space]
        [SerializeField] private GameObject prefabToSpawn;
    
        private float timer;

        private void Start()
        {
            timer = InitialDelay;
        }

        private void Update()
        {
            if (isPaused) return;

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Spawn();
                timer += SpawnInterval; 
            }
        }

        private void Spawn()
        {
            Instantiator.Spawn(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}
