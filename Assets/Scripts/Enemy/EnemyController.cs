using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool canChasePlayer = true; // whether the enemy can chase the player
    // if enemy can chase player if they see the enemy they will save transform and keep chasing them
    // they will attack when they are close enough
    //
    // chasing players cannot be drawn to the player, they will always chase the player if they see them
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
