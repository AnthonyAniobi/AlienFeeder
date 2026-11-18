using UnityEngine;

public class EnemyData
{
    public Vector3 position;
    public EnemyTypeEnum enemyType;

    public EnemyData(Vector3 position, EnemyTypeEnum enemyType)
    {
        this.position = position;
        this.enemyType = enemyType;
    }

    public EnemyData(float x, float y, EnemyTypeEnum enemyType)
    {
        this.position = new Vector3(x, 1, y);
        this.enemyType = enemyType;
    }

    // public EnemyData(Vector3 position, EnemyTypeEnum enemyType)
    // {
    //     this.position = position;
    //     this.enemyType = enemyType;
    // }
}

