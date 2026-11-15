using UnityEngine;

public class EdibleData
{
    public Vector3 position;
    public float value;

    public EdibleData(Vector3 position, float value)
    {
        this.position = position;
        this.value = value;
    }

    public EdibleData(float x, float y, float value)
    {
        position = new Vector3(x, 1, y);
        this.value = value;
    }
}
