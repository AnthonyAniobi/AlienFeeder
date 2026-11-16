using UnityEngine;

public class EnemyLookout : MonoBehaviour
{
    [SerializeField] private float lookoutDistance = 10f;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private float eyeRadius = 0.5f;
    [SerializeField] private float wallOffsetDistance = 1f;

    private Transform playerTransform;
    
    void Update()
    {
        if(playerTransform == null)
        {
            // moveTowardsPlayer();
            if(Physics.SphereCast(eyeTransform.position, 
            eyeRadius, 
            eyeTransform.forward, 
            out RaycastHit hitInfo, 
            lookoutDistance, 
            LayerMask.GetMask("Player", "Wall"))
            )
            {
                if(hitInfo.collider.CompareTag("Player"))
                {
                    if(playerTransform == null)
                    {
                        playerTransform = hitInfo.collider.transform;
                        GetComponent<EnemyController>().SetPlayerTransform(playerTransform);
                        Debug.Log("Mimic detected!");
                    }
                }
                if(hitInfo.collider.CompareTag("Wall"))
                {
                    
                }
                DebugExtension.DrawSphereCast(eyeTransform.position, eyeRadius, Vector3.forward, lookoutDistance, Color.green);
            }
            else
            {
                DebugExtension.DrawSphereCast(eyeTransform.position, eyeRadius, Vector3.forward, lookoutDistance, Color.red);
            }
        }
        // else
        // {
        //     // Move towards the player
        //     Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        //     Debug.DrawLine(transform.position, transform.position + directionToPlayer * lookoutDistance, Color.blue);
        // }
        
    }
}
