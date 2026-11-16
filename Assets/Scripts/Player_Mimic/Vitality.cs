using TMPro;
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
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI nutritionText;

    private float maxNutrition;
    public MimicLevelEnum MimicLevel => mimicLevel;
    private float currentHealth;
    private float currentNutrition;

    public float vitalityValue => mimicLevel.GetNutritionThreshold() + currentNutrition;

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

    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);
        UpdateSliders();
    }

    private void RemoveNutrition(float value)
    {
        if (value <= 0f)
            return;

        currentNutrition -= value;
        while (currentNutrition < 0f && mimicLevel > MimicLevelEnum.child)
        {
            mimicLevel--;
            currentNutrition += mimicLevel.GetNutritionThreshold();
            maxNutrition = mimicLevel.GetNutritionThreshold();
            GetComponent<Mimic>()?.ApplyMimicLevel(mimicLevel);
        }

        if (currentNutrition < 0f)
        {
            TakeDamage(-currentNutrition);
            currentNutrition = 0f;
        }

        UpdateSliders();
    }

    private void UpdateMimicLevel()
    {
        
        if (currentNutrition >= mimicLevel.GetNutritionThreshold()){
            int nextLevel = (int)mimicLevel + 1;
            if (nextLevel > (int)MimicLevelEnum.boss)
                return;

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
        if (nutritionBarFillImage != null)
            nutritionBarFillImage.color = Color.Lerp(Color.blue, Color.purple, currentNutrition / maxNutrition);
        if (healthBarFillImage != null)
            healthBarFillImage.color = Color.Lerp(Color.yellow, Color.green, currentHealth / maxHealth);

        if (nutritionText != null)
            nutritionText.text = $"{vitalityValue}";
        if (healthText != null)
            healthText.text = $"{currentHealth}/{maxHealth}";
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
                return 100f;
            default:
                return 0f;
        }
    }
}