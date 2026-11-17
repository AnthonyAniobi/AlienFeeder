using UnityEngine;

public class EnemyType : MonoBehaviour
{

    // Update is called once per frame
    public void Initialize(float speed, float attackPower, float attackRange)
    {
        GetComponent<EnemyAttack>().Initialize(attackPower, attackRange);
        GetComponent<EnemyMovement>().Initialize(speed);
    }
}
