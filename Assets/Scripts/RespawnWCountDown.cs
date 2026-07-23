using PSEMO.Events;
using UnityEngine;
using PSEMO.Player;

[RequireComponent(typeof(PlayerController))]
public class RespawnWCountDown : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
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
        playerController.Respawn();
    }
}