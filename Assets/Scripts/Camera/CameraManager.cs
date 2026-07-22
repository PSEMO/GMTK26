using System.Collections.Generic;
using UnityEngine;
using PSEMO.Events;

namespace PSEMO.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CameraSO data;

        private Dictionary<Transform, float> targets;
        private Vector3 velocity = Vector3.zero;

        private void Awake()
        {
            targets = new Dictionary<Transform, float>();
        }

        void Start()
        {
            transform.SetPositionAndRotation(GetTargetPos(), Quaternion.Euler(data.rotationOffset));
        }

        private void OnEnable()
        {
            CameraEvents.OnCameraTargetAdded += AddTarget;
            CameraEvents.OnCameraTargetRemoved += RemoveTarget;
            UIEvents.OnLoadingEnd += ResetToTarget;
        }

        private void OnDisable()
        {
            CameraEvents.OnCameraTargetAdded -= AddTarget;
            CameraEvents.OnCameraTargetRemoved -= RemoveTarget;
            UIEvents.OnLoadingEnd -= ResetToTarget;
        }

        void LateUpdate()
        {
            MoveTowardsTarget(GetTargetPos() + data.positionOffset);
        }

        private void MoveTowardsTarget(Vector3 targetPos)
        {
            Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, data.smoothTime, data.maxSpeed);

            if (data.useBounds)
            {
                newPos.x = Mathf.Clamp(newPos.x, data.minBounds.x, data.maxBounds.x);
                newPos.y = Mathf.Clamp(newPos.y, data.minBounds.y, data.maxBounds.y);
                newPos.z = Mathf.Clamp(newPos.z, data.minBounds.z, data.maxBounds.z);
            }

            transform.position = newPos;
        }

        private Vector3 GetTargetPos()
        {
            if (targets.Count > 0)
            {
                Vector3 endPosition = Vector3.zero;
                float totalWeight = 0f;

                foreach (Transform target in targets.Keys)
                {
                    float weight = targets[target];
                    endPosition += target.position * weight;
                    totalWeight += weight;
                }

                endPosition /= totalWeight;

                return new Vector3 (endPosition.x, endPosition.y, endPosition.z);
            }

            return transform.position;
        }

        public void AddTarget(Transform _transform, float weight)
        {
            targets.Add(_transform, weight);
        }

        public void RemoveTarget(Transform _tranform)
        {
            targets.Remove(_tranform);
        }

        private void ResetToTarget()
        {
            velocity = Vector3.zero;
            transform.SetPositionAndRotation(GetTargetPos(), Quaternion.Euler(data.rotationOffset));
        }
    }
}