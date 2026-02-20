using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler Instance { get; private set; }

    [Header("Scythe Stats")]
    [Range(1, 100)] public float scytheDamage = 10f;
    [Range(1, 50)] public float scytheWidth = 10f;
    [Range(1, 50)] public float scytheSpeed = 10f;

    [Header("Tomb Stats")]
    [Range(1, 200)] public int tombHealth = 10;
    [Range(1, 200)] public int tombMaxHealth = 10;
    [Range(1, 200)] public int tombHealthRegen = 1;

    [Header("Player Health")]
    [Range(1, 200)] public int playerHealth = 100;
    [Range(1, 200)] public int playerMaxHealth = 100;

    [Header("Player UI")]
    public Slider healthBar;

    [Header("Soul Value")]
    [Range(1, 20)] public int soulValue = 10;

    [Header("Upgrade Costs")]
    [Range(1, 999)] public int scytheDamageCost = 10;
    [Range(1, 999)] public int scytheWidthCost = 10;
    [Range(1, 999)] public int scytheSpeedCost = 10;
    [Range(1, 999)] public int tombHealthCost = 10;
    [Range(1, 999)] public int tombHealthRegenCost = 10;
    [Range(1, 999)] public int soulValueCost = 10;

    [Header("Values")]
    public int souls;
    public int wavesLeft;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    public void DamagePlayer(int amount)
    {
        playerHealth -= amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);
        UpdateHealthBar();
        if (playerHealth <= 0 && GameplayHandler.Instance != null)
        {
            GameplayHandler.Instance.GameOver();
        }
    }

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)playerHealth / playerMaxHealth;
        }
    }
}
