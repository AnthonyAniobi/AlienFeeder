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
        
        if (nutrition >= mimicLevel.GetNutritionThreshold()){
            int nextLevel = (int)mimicLevel + 1;
            mimicLevel = (MimicLevelEnum)nextLevel;
            nutrition = 0f;
            GetComponent<Mimic>()?.ApplyMimicLevel(mimicLevel);
        }
    }
}


public enum MimicLevelEnum
{
    child = 0,
    teen = 1,
    adult = 2,
    boss = 3
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
            case MimicLevelEnum.boss:
                return float.MaxValue;
            default:
                return 0f;
        }
    }
}