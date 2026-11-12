using UnityEngine;

public class Vitality : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float nutrition;
    [SerializeField] private MimicLevelEnum mimicLevel = MimicLevelEnum.child;

    public float Health => health;
    public float Nutrition => nutrition;
    public MimicLevelEnum MimicLevel => mimicLevel;

    private void Start()
    {
        UpdateMimicLevel();
    }

    public void AddNutrition(float value)
    {
        if (value <= 0f)
            return;

        nutrition += value;
        UpdateMimicLevel();
    }

    private void UpdateMimicLevel()
    {
        MimicLevelEnum newLevel = MimicLevelEnum.child;
        if (nutrition >= MimicLevel.GetNutritionThreshold())
            newLevel = MimicLevelEnum.adult;
        else if (nutrition >= MimicLevel.GetNutritionThreshold())
            newLevel = MimicLevelEnum.teen;

        if (newLevel == mimicLevel)
            return;

        mimicLevel = newLevel;
        GetComponent<MimicSpace.Mimic>()?.ApplyMimicLevel(mimicLevel);
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
    public static float GetNutritionThreshold(this MimicLevelEnum level)
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