using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float playerAttackOffsetDistance = 2f; // distance from the player to attack
    [SerializeField] private string wallTag = "Wall";
    [SerializeField] private float obstacleCheckDistance = 1.5f;
    [SerializeField] private float obstacleCheckRadius = 0.35f;
    [SerializeField] private float obstacleAvoidanceAngleStep = 30f;
    [SerializeField] private LayerMask obstacleLayers = Physics.AllLayers;
    // if enemy can chase player if they see the enemy they will save transform and keep chasing them
    // they will attack when they are close enough
    //
    // chasing players cannot be drawn to the player, they will always chase the player if they see them
    private Transform playerTransform; // reference to player's transform when found
    private float avoidanceAngle;
    internal Vector2 move;
    
    void Update()
    {
        if (playerTransform == null)
        {
            move = Vector2.zero;
            avoidanceAngle = 0f;
            return;
        }

        if(Vector3.Distance(playerTransform.position, transform.position) <= playerAttackOffsetDistance)
        {
            GetComponent<EnemyAttack>()?.Attack();
            move = Vector2.zero; // stop moving when attacking
            avoidanceAngle = 0f;
            return;
        }

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        if (!IsWallBlocking(directionToPlayer, obstacleCheckDistance))
        {
            avoidanceAngle = 0f;
        }
        else
        {
            if (avoidanceAngle == 0f)
            {
                avoidanceAngle = ChooseAvoidanceAngle(directionToPlayer);
            }

            Vector3 aroundWall = Quaternion.Euler(0f, avoidanceAngle, 0f) * directionToPlayer;
            if (!IsWallBlocking(aroundWall, obstacleCheckDistance))
            {
                directionToPlayer = aroundWall;
            }
        }

        move = new Vector2(directionToPlayer.x, directionToPlayer.z);
    }

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    private float ChooseAvoidanceAngle(Vector3 directionToPlayer)
    {
        float angleStep = Mathf.Clamp(obstacleAvoidanceAngleStep, 1f, 90f);
        for (float angle = angleStep; angle <= 90f; angle += angleStep)
        {
            if (!IsWallBlocking(Quaternion.Euler(0f, angle, 0f) * directionToPlayer, obstacleCheckDistance))
            {
                return angle;
            }

            if (!IsWallBlocking(Quaternion.Euler(0f, -angle, 0f) * directionToPlayer, obstacleCheckDistance))
            {
                return -angle;
            }
        }

        return angleStep;
    }

    private bool IsWallBlocking(Vector3 direction, float distance)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            return false;
        }

        Vector3 origin = transform.position + Vector3.up * obstacleCheckRadius;
        RaycastHit[] hits = Physics.SphereCastAll(
            origin,
            obstacleCheckRadius,
            direction.normalized,
            distance,
            obstacleLayers.value == 0 ? Physics.AllLayers : obstacleLayers.value);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag(wallTag))
            {
                return true;
            }
        }

        return false;
    }
}
