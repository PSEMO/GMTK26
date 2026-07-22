using UnityEngine;

namespace PSEMO.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player")]
    public class PlayerSO : ScriptableObject
    {
        [Header("Camera")]
        public float camWeight = 1;
        
        [Space]

        [Header("Movement")]
        public float speed = 8f;

        [Space]

        [Header("Jump")]
        public float jumpForce = 14f;
        public int maxJumpCount = 1;
        public float jumpBuffer = 0.16f;
        public float coyoteJump = 0.16f;
        
        [Header("Config")]
        public float groundCheckDistance = 0.02f;
        public LayerMask groundLayer = 1152;
        
        [Space]

        [Header("Dash")]
        public float dashForce = 40f;
        public float dashDuration = 0.12f;

        [Header("Config")]
        public float wallCheckDistance = 0.2f;
        public LayerMask wallLayer = 128;

        [Space]

        [Header("Interaction")]
        public float interactionRadius = 4f;
        public LayerMask interactionLayer = 64;
        
        [Space]

        [Header("AbleTo")]
        public bool ableToInteract = true;
        public bool ableToMove = true;
        public bool ableToJump = true;
        public bool ableToDash = true;
    }
}