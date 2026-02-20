using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("Game State")]
    public float timeLeft = 60f;
    public int currentSkulls = 0;
    public float defaultRoundTime = 60f;
    
    [Header("UI References (Optional)")]
    public TextMeshProUGUI skullCountText;
    public TextMeshProUGUI wavesLeftText;
    
    public TextMeshProUGUI scytheDamageButtonText;
    public TextMeshProUGUI scytheWidthButtonText;
    public TextMeshProUGUI scytheSpeedButtonText;
    public TextMeshProUGUI tombHealthButtonText;
    public TextMeshProUGUI tombHealthRegenButtonText;
    public TextMeshProUGUI soulValueButtonText;
    
    [Header("Round Events")]
    public UnityEvent onRoundStart;
    public UnityEvent onRoundStop;

    [SerializeField] private VerticalLayoutGroup layout;
    
    private int scytheDamageUpgrades = 0;
    private int scytheWidthUpgrades = 0;
    private int scytheSpeedUpgrades = 0;
    private int tombHealthUpgrades = 0;
    private int tombHealthRegenUpgrades = 0;
    private int soulValueUpgrades = 0;
    
    private bool roundActive = false;
    
    public int wavesLeft = 0;

    void Start()
    {
        UpdateSkullDisplay();
    }

    void Update()
    {
        if (roundActive && timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            
            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                StopRound();
            }
        }
    }

    public void ModifySkulls(int amount)
    {
        currentSkulls += amount;
        currentSkulls = Mathf.Max(0, currentSkulls); 
        UpdateSkullDisplay();
    }

    public void StopRound()
    {
        roundActive = false;
        Debug.Log("Round stopped!");
        onRoundStop.Invoke();
    }

    private int CalculateUpgradeCost(int baseCost, int upgrades)
    {
        return Mathf.Max(baseCost, Mathf.RoundToInt(Mathf.Pow(upgrades, 3) / 10f)+10);
    }
    
    void UpdateWaveDisplay()
    {
        if (wavesLeftText != null)
            wavesLeftText.text = $"Waves left: {wavesLeft}";
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());

    }

    public void SetWaveCount(int count)
    {
        wavesLeft = count;
        UpdateWaveDisplay();
    }

    public void DecrementWave()
    {
        wavesLeft = Mathf.Max(0, wavesLeft - 1);
        UpdateWaveDisplay();
    }
    
    void UpdateSkullDisplay()
    {
        if (skullCountText != null)
        {
            skullCountText.text = $"Souls: {currentSkulls}";
        }
    }
    
    public void UpdateUpgradeButtonTexts()
    {
        if (scytheDamageButtonText != null)
            scytheDamageButtonText.text = $"{PlayerHandler.Instance.scytheDamageCost}";
        if (scytheWidthButtonText != null)
            scytheWidthButtonText.text = $"{PlayerHandler.Instance.scytheWidthCost}";
        if (scytheSpeedButtonText != null)
            scytheSpeedButtonText.text = $"{PlayerHandler.Instance.scytheSpeedCost}";
        if (tombHealthButtonText != null)
            tombHealthButtonText.text = $"{PlayerHandler.Instance.tombHealthCost}";
        if (tombHealthRegenButtonText != null)
            tombHealthRegenButtonText.text = $"{PlayerHandler.Instance.tombHealthRegenCost}";
        if (soulValueButtonText != null)
            soulValueButtonText.text = $"{PlayerHandler.Instance.soulValueCost}";
    }
    
    public void UpgradeScytheDamage()
    {
        if (currentSkulls >= PlayerHandler.Instance.scytheDamageCost)
        {
            currentSkulls -= PlayerHandler.Instance.scytheDamageCost;
            PlayerHandler.Instance.scytheDamage += 10f;
            scytheDamageUpgrades++;
            PlayerHandler.Instance.scytheDamageCost = CalculateUpgradeCost(PlayerHandler.Instance.scytheDamageCost, scytheDamageUpgrades);
            UpdateSkullDisplay();
            UpdateUpgradeButtonTexts();
        }
    }


    public void UpgradeScytheWidth()
    {
        if (currentSkulls >= PlayerHandler.Instance.scytheWidthCost)
        {
            currentSkulls -= PlayerHandler.Instance.scytheWidthCost;
            PlayerHandler.Instance.scytheWidth += 0.1f;
            scytheWidthUpgrades++;
            PlayerHandler.Instance.scytheWidthCost = CalculateUpgradeCost(PlayerHandler.Instance.scytheWidthCost, scytheWidthUpgrades);
            UpdateSkullDisplay();
            UpdateUpgradeButtonTexts();
        }
    }


    public void UpgradeScytheSpeed()
    {
        if (currentSkulls >= PlayerHandler.Instance.scytheSpeedCost)
        {
            currentSkulls -= PlayerHandler.Instance.scytheSpeedCost;
            PlayerHandler.Instance.scytheSpeed += 5f;
            scytheSpeedUpgrades++;
            PlayerHandler.Instance.scytheSpeedCost = CalculateUpgradeCost(PlayerHandler.Instance.scytheSpeedCost, scytheSpeedUpgrades);
            UpdateSkullDisplay();
            UpdateUpgradeButtonTexts();
        }
    }

    public void UpgradeTombHealth()
    {
        if (currentSkulls >= PlayerHandler.Instance.tombHealthCost)
        {
            currentSkulls -= PlayerHandler.Instance.tombHealthCost;
            PlayerHandler.Instance.tombHealth += 10;
            PlayerHandler.Instance.tombMaxHealth += 10;
            tombHealthUpgrades++;
            PlayerHandler.Instance.tombHealthCost = CalculateUpgradeCost(PlayerHandler.Instance.tombHealthCost, tombHealthUpgrades);
            
            UpdateSkullDisplay();
            UpdateUpgradeButtonTexts();
        }
    }

    public void UpgradeTombHealthRegeneration()
    {
        if (currentSkulls >= PlayerHandler.Instance.tombHealthRegenCost)
        {
            currentSkulls -= PlayerHandler.Instance.tombHealthRegenCost;
            PlayerHandler.Instance.tombHealthRegen += 10;
            tombHealthRegenUpgrades++;
            PlayerHandler.Instance.tombHealthRegenCost = CalculateUpgradeCost(PlayerHandler.Instance.tombHealthRegenCost, tombHealthRegenUpgrades);
            
            UpdateSkullDisplay();
            UpdateUpgradeButtonTexts();
        }
    }
    public void UpgradeSoulValue()
    {
        if (currentSkulls >= PlayerHandler.Instance.soulValueCost)
        {
            currentSkulls -= PlayerHandler.Instance.soulValueCost;
            PlayerHandler.Instance.soulValue += 1;
            soulValueUpgrades++;
            PlayerHandler.Instance.soulValueCost = CalculateUpgradeCost(PlayerHandler.Instance.soulValueCost, soulValueUpgrades);
            
            UpdateSkullDisplay();
            UpdateUpgradeButtonTexts();
        }
    }
}
