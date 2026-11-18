using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public void Initialize(EnemyTypeEnum enemyType
    )
    {
        GetComponent<EnemyAttack>().Initialize(
            enemyType.GetAttackPower(), 
            enemyType.GetAttackRange()
        );
        GetComponent<EnemyMovement>().Initialize(
            enemyType.GetSpeed()
        );
    }
}

public enum EnemyTypeEnum
{
    basic,
    medium,
    advanced,
}

public static class EnumTypeExtensions
{
    public static float GetSpeed(this EnemyTypeEnum enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypeEnum.basic:
                return 4f;
            case EnemyTypeEnum.medium:
                return 6f;
            case EnemyTypeEnum.advanced:
                return 7f;
            default:
                return 5f;
        }
    }

     public static float GetAttackPower(this EnemyTypeEnum enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypeEnum.basic:
                return 5f;
            case EnemyTypeEnum.medium:
                return 7f;
            case EnemyTypeEnum.advanced:
                return 10f;
            default:
                return 5f;
        }
    }

    public static float GetAttackRange(this EnemyTypeEnum enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypeEnum.basic:
                return 5f;
            case EnemyTypeEnum.medium:
                return 7f;
            case EnemyTypeEnum.advanced:
                return 10f;
            default:
                return 5f;
        }
    }
}
