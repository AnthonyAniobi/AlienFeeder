using UnityEngine;

public class EnemyLookout : MonoBehaviour
{
    [SerializeField] private float lookoutDistance = 10f;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private float eyeRadius = 0.5f;
    [SerializeField] private LayerMask lookoutLayers = Physics.AllLayers;

    void Update()
    {
        if (eyeTransform == null)
        {
            return;
        }

        DebugExtension.DrawSphereCast(
            eyeTransform.position,
            eyeRadius,
            eyeTransform.forward,
            lookoutDistance,
            FindPlayer() != null ? Color.green : Color.red);
    }

    private Transform FindPlayer()
    {
        int layerMask = lookoutLayers.value == 0 ? Physics.AllLayers : lookoutLayers.value;
        RaycastHit[] hits = Physics.SphereCastAll(
            eyeTransform.position,
            eyeRadius,
            eyeTransform.forward,
            lookoutDistance,
            layerMask);

        System.Array.Sort(hits, (first, second) => first.distance.CompareTo(second.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform))
            {
                continue;
            }

            if (hit.collider.CompareTag("Wall"))
            {
                return null;
            }

            Mimic mimic = hit.collider.GetComponentInParent<Mimic>();
            if (hit.collider.CompareTag("Player") || mimic != null)
            {
                Transform player = mimic != null ? mimic.transform : hit.collider.transform;
                GetComponent<EnemyController>()?.SetPlayerTransform(player);
                return player;
            }
        }

        return null;
    }
}
