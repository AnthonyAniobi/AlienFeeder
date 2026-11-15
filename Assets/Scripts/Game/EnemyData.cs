using UnityEngine;

public class EnemyData
{
    public Vector3 position;
    public float power;
    public float damage;
    public bool isMoving;

    public EnemyData(Vector3 position, float power, float damage, bool isMoving)
    {
        this.position = position;
        this.power = power;
        this.damage = damage;
        this.isMoving = isMoving;
    }

    public EnemyData(float x, float y, float power, float damage, bool isMoving)
    {
        this.position = new Vector3(x, 1, y);
        this.power = power;
        this.damage = damage;
        this.isMoving = false;
    }

    public EnemyData(Vector3 position, float power, float damage)
    {
        this.position = position;
        this.power = power;
        this.damage = damage;
        this.isMoving = false;
    }
}

