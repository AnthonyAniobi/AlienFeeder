using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 5.5f;


    public void Attack()
    {
        Transform currentAttackPoint = attackPoint != null ? attackPoint : transform;
        Collider[] hitColliders = Physics.OverlapSphere(currentAttackPoint.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            Vitality targetVitality = hitCollider.GetComponentInParent<Vitality>();
            if (targetVitality != null)
            {
                targetVitality.TakeDamage(attackDamage);
                break;
            }
        }
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
            targetVitality.ResolveEnemyContact(attackDamage, 0, transform.position, 0f);
        }
    }
}
