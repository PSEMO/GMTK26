using UnityEngine;
using PSEMO.Core.Management;

public class Shooter : MonoBehaviour, IPausable
{
    private bool isPaused = false;
    public void Pause() => isPaused = true;
    public void Continue() => isPaused = false;

    [SerializeField] GameObject projectile;

    [SerializeField] Transform barrelStart;
    [SerializeField] Transform barrelEnd;

    [SerializeField] float speed = 1;

    [SerializeField] float timeBetweenShoot = 1;

    float timer = 0;

    void Update()
    {
        if (isPaused) return;

        timer += Time.deltaTime;

        if (timer > timeBetweenShoot)
        {
            timer -= timeBetweenShoot;

            Vector3 direction = (barrelEnd.position - barrelStart.position).normalized;

            Projectile ctx = Instantiator.Spawn(projectile, barrelEnd.position, Quaternion.LookRotation(direction, Vector3.forward)).GetComponent<Projectile>();
            ctx.SetSpeedAndDirection(speed, direction);
        }
    }
}