using PSEMO.Environment.Functionality;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable, IPausable
{
    private bool isPaused = false;
    public void Pause() => isPaused = true;
    public void Continue() => isPaused = false;

    private Vector3 direction = Vector3.zero;
    private float speed = 0;
    
    public void ResetObject()
    {
        direction = Vector3.zero;
        speed = 0;
        isPaused = false;
    }

    public void SetSpeedAndDirection(float newSpeed, Vector3 newDirection)
    {
        direction = newDirection;
        speed = newSpeed;
    }
    
    void Update()
    {
        if (isPaused) return;

        transform.position += speed * Time.deltaTime * direction;
    }
}