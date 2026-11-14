using UnityEngine;

public class EnemyLookout : MonoBehaviour
{
    [SerializeField] private float lookoutDistance = 10f;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private float eyeRadius = 0.5f;

    private Transform playerTransform;
    
    void Update()
    {
        if(playerTransform == null)
        {
            // moveTowardsPlayer();
            if(Physics.SphereCast(eyeTransform.position, eyeRadius, eyeTransform.forward, out RaycastHit hitInfo, lookoutDistance))
            {
                if(hitInfo.collider.CompareTag("Player"))
                {
                    if(playerTransform == null)
                    {
                        playerTransform = hitInfo.collider.transform;
                        Debug.Log("Mimic detected!");
                    }
                }
                if(hitInfo.collider.CompareTag("Wall"))
                {
                    Debug.Log("Mimic detected!");
                }
            }
        }
        else
        {
            // Move towards the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            
        }
        
    }
}
