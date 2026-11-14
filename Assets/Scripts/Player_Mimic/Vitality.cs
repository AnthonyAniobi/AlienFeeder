using UnityEngine;
using UnityEngine.UI;

public class Vitality : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxNutrition = 100f;
    [SerializeField] private float nutrition;
    [SerializeField] private MimicLevelEnum mimicLevel = MimicLevelEnum.child;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Slider nutritionBarSlider;
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image nutritionBarFillImage;

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
        currentNutrition = maxNutrition;
        UpdateSliders();
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
        nutritionBarFillImage.color = Color.Lerp(Color.red, Color.green, currentNutrition / maxNutrition);
        healthBarFillImage.color = Color.Lerp(Color.red, Color.green, currentHealth / maxHealth);
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