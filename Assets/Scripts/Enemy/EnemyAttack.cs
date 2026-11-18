using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 5.5f;
    [SerializeField] private float attackCooldown = 1f;
    private float nextAttackTime;

    public void Initialize(float attackPower, float range)
    {
        attackDamage = attackPower;
        attackRange = range;   
    }



    void OnTriggerEnter(Collider other)
    {
        DamagePlayer(other);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     DamagePlayer(collision.collider);
    // }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        DamagePlayer(hit.collider);
    }

    private void DamagePlayer(Collider other)
    {
        // if (Time.time < nextAttackTime)
        //     return;

        Vitality targetVitality = other.GetComponentInParent<Vitality>();
        // Vitality targetVitality = other.GetComponent<Vitality>();
        if (targetVitality != null)
        {
            nextAttackTime = Time.time + attackCooldown;
            targetVitality.ResolveEnemyContact(attackDamage, transform.position, 3f);
        }
    }
}
