using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    //
    private Transform playerTransform; // reference to player's transform when found
    internal Vector2 move;
    
    void Update()
    {
        if(playerTransform != null)
        {
            move = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.z - transform.position.z).normalized;
        }
    }
}
