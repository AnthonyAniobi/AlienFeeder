using UnityEngine;

public class RocketPartData
{
    public Vector3 position;

    public RocketPartData(Vector3 position)
    {
        this.position = position;
    }

    public RocketPartData(float x, float y)
    {
        position = new Vector3(x, 1, y);
    }
}
