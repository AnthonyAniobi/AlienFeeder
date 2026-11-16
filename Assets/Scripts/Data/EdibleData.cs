using UnityEngine;

public class EdibleData
{
    public Vector3 position;
    public EdibleType type;

    public EdibleData(Vector3 position, EdibleType type)
    {
        this.position = position;
        this.type = type;
    }

    public EdibleData(float x, float y, EdibleType type)
    {
        position = new Vector3(x, 1, y);
        this.type = type;
    }
}


public enum EdibleType
{
    Apple,
    Banana
}
