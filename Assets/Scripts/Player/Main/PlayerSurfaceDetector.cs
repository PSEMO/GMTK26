using UnityEngine;

namespace PSEMO.Player
{
    public class PlayerSurfaceDetector
    {
        private readonly Vector3 groundCheckBoxSize;
        private readonly Vector3 wallCheckBoxSize;

        private readonly LayerMask groundLayer;
        private readonly LayerMask wallLayer;

        private readonly float groundCheckDistance;
        private readonly float wallCheckDistance;

        public PlayerSurfaceDetector(Collider col, PlayerSO data)
        {
            groundCheckBoxSize = col.bounds.size * 0.9f;
            wallCheckBoxSize = col.bounds.size * 0.9f;

            groundCheckDistance = data.groundCheckDistance;
            wallCheckDistance = data.wallCheckDistance;

            groundLayer = data.groundLayer;
            wallLayer = data.wallLayer;
        }

        public bool IsOnGround(Vector3 center)
        {
            float gravitySign = Mathf.Sign(Physics.gravity.y);
            center.y += gravitySign * groundCheckDistance;

            return Physics.CheckBox(center, groundCheckBoxSize / 2f, Quaternion.identity, groundLayer);
        }

        public bool IsFacingWall(Vector3 center, Vector3 direction)
        {
            center += direction * wallCheckDistance;

            return Physics.CheckBox(center, wallCheckBoxSize / 2f, Quaternion.identity, wallLayer);
        }
    }
}
