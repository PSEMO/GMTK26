using UnityEngine;
using FMODUnity;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private EventReference jumpEvent;
    [SerializeField] private EventReference dashEvent;
    [SerializeField] private EventReference footstepEvent;
    [SerializeField] private EventReference landEvent;

    // Called in events
    public void PlayJump()      => RuntimeManager.PlayOneShotAttached(jumpEvent, gameObject);
    public void PlayDash()      => RuntimeManager.PlayOneShotAttached(dashEvent, gameObject);
    public void PlayFootstep()  => RuntimeManager.PlayOneShotAttached(footstepEvent, gameObject);
    public void PlayLand()      => RuntimeManager.PlayOneShotAttached(landEvent, gameObject);
}