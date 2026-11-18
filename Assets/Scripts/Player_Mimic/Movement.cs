using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 0.8f;
        public float speed = 5f;
        [Header("Camera Look")]
        [Tooltip("Transform rotated by mouse or stick look input. Defaults to this Mimic transform.")]
        [SerializeField] private Transform cameraLookTarget;
        [Tooltip("Mouse look speed and analogue stick look speed")]
        [Range(0.1f, 20f)]
        public float lookSensitivity = 1f;
        [Tooltip("How far in degrees you can look up")]
        [Range(0f, 89f)]
        public float topClamp = 70f;
        [Tooltip("How far in degrees you can look down")]
        [Range(-89f, 0f)]
        public float bottomClamp = -30f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;
        private float cameraTargetYaw;
        private float cameraTargetPitch;
        private const float lookThreshold = 0.01f;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            if (cameraLookTarget == null)
            {
                cameraLookTarget = transform;
            }

            cameraTargetYaw = cameraLookTarget.eulerAngles.y;
            cameraTargetPitch = ClampAngle(cameraLookTarget.eulerAngles.x, bottomClamp, topClamp);
        }

        void Update()
        {
            Vector3 inputDirection = new Vector3(CustomInput.Instance.move.x, 0f, CustomInput.Instance.move.y);
            Vector3 cameraRelativeDirection = Quaternion.Euler(0f, cameraTargetYaw, 0f) * inputDirection.normalized;
            velocity = Vector3.Lerp(velocity, cameraRelativeDirection * speed, velocityLerpCoef * Time.deltaTime);

            // Assigning velocity to the mimic to assure great leg placement
            myMimic.velocity = velocity;

            Vector3 movement = velocity * Time.deltaTime;
            if (myMimic.IsWallBlocking(movement))
                velocity = Vector3.zero;
            else
                transform.position += movement;

            // A blocked mimic must report zero velocity so its legs do not step through the wall.
            myMimic.velocity = velocity;
            Vector3 destHeight = transform.position;
            if (myMimic.TryGetGroundPoint(transform.position, out Vector3 groundPoint))
                destHeight = new Vector3(transform.position.x, groundPoint.y + height, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (CustomInput.Instance == null)
                return;

            Vector2 look = CustomInput.Instance.look;
            if (look.sqrMagnitude >= lookThreshold)
            {
                float deltaTimeMultiplier = 1f;
                if (Mouse.current == null || Mouse.current.delta.ReadValue().sqrMagnitude < lookThreshold)
                {
                    deltaTimeMultiplier = Time.deltaTime;
                }

                cameraTargetYaw += look.x * lookSensitivity * deltaTimeMultiplier;
                cameraTargetPitch += look.y * lookSensitivity * deltaTimeMultiplier;
            }

            cameraTargetYaw = ClampAngle(cameraTargetYaw, float.MinValue, float.MaxValue);
            cameraTargetPitch = ClampAngle(cameraTargetPitch, bottomClamp, topClamp);
            cameraLookTarget.rotation = Quaternion.Euler(cameraTargetPitch, cameraTargetYaw, 0f);
        }

        private static float ClampAngle(float angle, float minimum, float maximum)
        {
            angle %= 360f;
            if (angle < -360f)
                angle += 360f;
            if (angle > 360f)
                angle -= 360f;
            return Mathf.Clamp(angle, minimum, maximum);
        }
    }

}