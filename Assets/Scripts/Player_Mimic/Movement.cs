using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
        }

        void Update()
        {
            velocity = Vector3.Lerp(velocity, new Vector3(CustomInput.Instance.move.x, 0, CustomInput.Instance.move.y).normalized * speed, velocityLerpCoef * Time.deltaTime);

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
    }

}