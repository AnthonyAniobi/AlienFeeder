using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Vitality : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Slider batteriesBarSlider;
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image batteriesBarFillImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI batteriesText;

    private float maxBatteries;
    private float currentHealth;
    private float currentBatteries;

    // public float vitalityValue => mimicLevel.GetNutritionThreshold() + currentBatteries;

    private void Start()
    {
        Initialize(maxHealth, maxBatteries);
    }

    public void Initialize(float health, float batteries)
    {
        maxHealth = health;
        maxBatteries = batteries;
        currentHealth = maxHealth;
        // maxBatteries = mimicLevel.GetNutritionThreshold();
        currentBatteries = 0;
        UpdateSliders();
    }

    public void AddNutrition(float value)
    {
        if (value <= 0f)
            return;

        currentBatteries += value;
        UpdateSliders();
        if (currentBatteries >= maxBatteries)
        {
            FindObjectOfType<GameLevelManager>()?.OnPlayerVictory();
        }
    }


    public bool ResolveEnemyContact(float damage, float nutrition,  Vector3 enemyPosition, float pushbackDistance)
    {
        RemoveNutrition(nutrition);
        TakeDamage(damage);

        Vector3 pushDirection = transform.position - enemyPosition;
        pushDirection.y = 0f;
        if (pushDirection.sqrMagnitude > 0f)
            transform.position += pushDirection.normalized * pushbackDistance;

        return false;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);
        UpdateSliders();
        FindObjectOfType<GameLevelManager>()?.OnPlayerDeath();
    }

    private void RemoveNutrition(float value)
    {
        if (value <= 0f)
            return;

        currentBatteries -= value;
        
        if (currentBatteries < 0f)
        {
            TakeDamage(-currentBatteries);
            currentBatteries = 0f;
        }

        UpdateSliders();
    }


    private void UpdateSliders()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth / maxHealth;
        }

        if (batteriesBarSlider != null)
        {
            batteriesBarSlider.value = currentBatteries / maxBatteries;
        }
        if (batteriesBarFillImage != null)
            batteriesBarFillImage.color = Color.Lerp(Color.blue, Color.purple, currentBatteries / maxBatteries);
        if (healthBarFillImage != null)
            healthBarFillImage.color = Color.Lerp(Color.yellow, Color.green, currentHealth / maxHealth);

        if (batteriesText != null)
            batteriesText.text = $"{currentBatteries}/{maxBatteries}";
        if (healthText != null)
            healthText.text = $"{currentHealth}/{maxHealth}";
    }
}

