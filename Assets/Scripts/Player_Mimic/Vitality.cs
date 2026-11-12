using UnityEngine;

public class Vitality : MonoBehaviour
{
    public float health;
    public float nutrition;
    public MimicLevelEnum mimicLevel = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public enum MimicLevelEnum
{
    child = 0,
    teen = 1,
    adult = 2
}

public static class MimicLevel
{
    public static float GetNutritionThreshold(MimicLevelEnum level)
    {
        switch (level)
        {
            case MimicLevelEnum.child:
                return 10f;
            case MimicLevelEnum.teen:
                return 20f;
            case MimicLevelEnum.adult:
                return 30f;
            default:
                return 0f;
        }
    }
}