using UnityEngine;
using UnityEngine.UI;

public class Vitality : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private MimicLevelEnum mimicLevel = MimicLevelEnum.child;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Slider nutritionBarSlider;
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image nutritionBarFillImage;

    private float maxNutrition;
    public MimicLevelEnum MimicLevel => mimicLevel;
    private float currentHealth;
    private float currentNutrition;

    private void Start()
    {
        UpdateMimicLevel();
        Initialize(maxHealth, maxNutrition);
    }

    public void Initialize(float health, float nutrition)
    {
        maxHealth = health;
        maxNutrition = nutrition;
        currentHealth = maxHealth;
        maxNutrition = mimicLevel.GetNutritionThreshold();
        currentNutrition = 0;
        UpdateSliders();
    }

    public void AddNutrition(float value)
    {
        if (value <= 0f)
            return;

        currentNutrition += value;
        UpdateMimicLevel();
        UpdateSliders();
    }

    private void UpdateMimicLevel()
    {
        
        if (currentNutrition >= mimicLevel.GetNutritionThreshold()){
            int nextLevel = (int)mimicLevel + 1;
            mimicLevel = (MimicLevelEnum)nextLevel;
            currentNutrition = 0f;
            maxNutrition = mimicLevel.GetNutritionThreshold();
            GetComponent<Mimic>()?.ApplyMimicLevel(mimicLevel);
        }
    }

    private void UpdateSliders()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth / maxHealth;
        }

        if (nutritionBarSlider != null)
        {
            nutritionBarSlider.value = currentNutrition / maxNutrition;
        }
        nutritionBarFillImage.color = Color.Lerp(Color.blue, Color.purple, currentNutrition / maxNutrition);
        healthBarFillImage.color = Color.Lerp(Color.yellow, Color.green, currentHealth / maxHealth);
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