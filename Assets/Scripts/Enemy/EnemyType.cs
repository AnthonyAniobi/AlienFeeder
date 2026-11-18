using UnityEngine;

public class EnemyType : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
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
        if (meshRenderer != null)
        {
            meshRenderer.material.color = enemyType.GetColor();
        }
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
                return 3.8f;
            case EnemyTypeEnum.medium:
                return 5f;
            case EnemyTypeEnum.advanced:
                return 6.5f;
            default:
                return 5f;
        }
    }

     public static float GetAttackPower(this EnemyTypeEnum enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypeEnum.basic:
                return 20f;
            case EnemyTypeEnum.medium:
                return 30f;
            case EnemyTypeEnum.advanced:
                return 50f;
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

    public static Color GetColor(this EnemyTypeEnum enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypeEnum.basic:
                return Color.blue;
            case EnemyTypeEnum.medium:
                return Color.orange;
            case EnemyTypeEnum.advanced:
                return Color.red;
            default:
                return Color.blue;
        }
    }
}
