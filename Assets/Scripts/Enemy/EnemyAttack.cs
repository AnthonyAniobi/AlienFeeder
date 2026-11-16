using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;


    public void Attack()
    {
        Transform currentAttackPoint = attackPoint != null ? attackPoint : transform;
        Collider[] hitColliders = Physics.OverlapSphere(currentAttackPoint.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Vitality targetVitality = hitCollider.GetComponentInParent<Vitality>();
                targetVitality?.TakeDamage(attackDamage);

                break;
            }
        }
    }
}
