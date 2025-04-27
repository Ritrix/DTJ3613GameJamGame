using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    private int HealthIncreaseCost;
    private int SpeedIncreaseCost;
    private int DamageIncreaseCost;

    public TMP_Text increaseHealthPriceTextBox;
    public TMP_Text increaseSpeedPriceTextBox;
    public TMP_Text increaseDamagePriceTextBox;

    public TMP_Text gold;
    private void Awake()
    {
        
    }

    private void Start()
    {
        increaseHealthPriceTextBox = GameObject.Find("IncreaseHealthPrice").GetComponent<TMP_Text>();
        increaseSpeedPriceTextBox = GameObject.Find("IncreaseSpeedPrice").GetComponent<TMP_Text>();
        increaseDamagePriceTextBox = GameObject.Find("IncreaseDamagePrice").GetComponent<TMP_Text>();
        gold = GameObject.Find("GoldAmount").GetComponent<TMP_Text>();

        HealthIncreaseCost = 75 * GameManager.Instance.currentPurchaseMod;
        SpeedIncreaseCost = 75 * GameManager.Instance.currentPurchaseMod;
        DamageIncreaseCost = 150 * GameManager.Instance.currentPurchaseMod;

        increaseHealthPriceTextBox.text = HealthIncreaseCost.ToString();
        increaseSpeedPriceTextBox.text = SpeedIncreaseCost.ToString();
        increaseDamagePriceTextBox.text = DamageIncreaseCost.ToString();

        if (GameManager.Instance.playerCurrentMaxHealth >= GameManager.Instance.playerMaxHealth)
        {
            increaseHealthPriceTextBox.text = "MAX";
        }
        if (GameManager.Instance.playerCurrentMaxSpeed >= GameManager.Instance.playerSpeedMax)
        {
            increaseSpeedPriceTextBox.text = "MAX";
        }
        if (GameManager.Instance.playerCurrentMaxDamage >= GameManager.Instance.playerDamageMax)
        {
            increaseDamagePriceTextBox.text = "MAX";
        }
    }
    public void increaseHealthClicked()
    {
        if (GameManager.Instance.playerGold >= HealthIncreaseCost && GameManager.Instance.playerCurrentMaxHealth < GameManager.Instance.playerMaxHealth)
        {
            GameManager.Instance.playerGold -= HealthIncreaseCost;
            GameManager.Instance.IncreaseHealth(1);
        }
    }

    public void increaseSpeedClicked()
    {
        if (GameManager.Instance.playerGold >= SpeedIncreaseCost && GameManager.Instance.playerCurrentMaxSpeed < GameManager.Instance.playerSpeedMax)
        {
            GameManager.Instance.playerGold -= SpeedIncreaseCost;
            GameManager.Instance.IncreaseSpeed(1);
        }
    }

    public void increaseDamageClicked()
    {
        if (GameManager.Instance.playerGold >= DamageIncreaseCost && GameManager.Instance.playerCurrentMaxDamage < GameManager.Instance.playerDamageMax)
        {
            GameManager.Instance.playerGold -= DamageIncreaseCost;
            GameManager.Instance.IncreaseDamage(1);
        }
    }

    private void FixedUpdate()
    {
        gold.text = "Gold: " + GameManager.Instance.playerGold.ToString();
    }

    public void NextWave()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
