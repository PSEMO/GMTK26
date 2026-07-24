using PSEMO.Environment.Functionality;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    private Vector3 direction = Vector3.zero;
    private float speed = 0;
    
    public void ResetObject()
    {
        direction = Vector3.zero;
        speed = 0;
    }

    public void SetSpeedAndDirection(float newSpeed, Vector3 newDirection)
    {
        direction = newDirection;
        speed = newSpeed;
    }
    
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }
}