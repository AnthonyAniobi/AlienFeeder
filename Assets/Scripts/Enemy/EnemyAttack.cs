using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 5.5f;

    public void Initialize(float attackPower, float range)
    {
        attackDamage = attackPower;
        attackRange = range;   
    }



    void OnTriggerEnter(Collider other)
    {
        DamagePlayer(other);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        DamagePlayer(hit.collider);
    }

    private void DamagePlayer(Collider other)
    {
        Vitality targetVitality = other.GetComponentInParent<Vitality>();
        if (targetVitality != null)
        {
            targetVitality.ResolveEnemyContact(attackDamage, transform.position, 3f);
        }
    }
}
