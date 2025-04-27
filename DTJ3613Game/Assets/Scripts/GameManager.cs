using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentWave = 1;
    public int playerGold = 0;
    public int playerMaxHealth = 20;
    public int playerSpeedMax = 16;
    public int playerDamageMax = 3;
    
    public int playerCurrentMaxHealth = 5;
    public int playerCurrentMaxSpeed = 8;
    public int playerCurrentMaxDamage = 0;

    public int currentPurchaseMod = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNextWave()
    {
        currentWave++;
        SceneManager.LoadScene("SampleScene"); // reloads or continues
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        Debug.Log("Gold: " + playerGold);
    }

    public void ResetGame()
    {
        playerCurrentMaxHealth = 5;
        playerCurrentMaxSpeed = 8;
        playerCurrentMaxDamage = 0;
        currentWave = 1;
        playerGold = 0;
        SceneManager.LoadScene("SampleScene");
    }

    public void IncreaseHealth(int amount)
    {
        playerMaxHealth += amount;
        Debug.Log("Player Max Health: " + playerMaxHealth);
    }

    public void IncreaseSpeed(int amount)
    {
        playerSpeedMax += amount;
        Debug.Log("Player Speed: " + playerSpeedMax);
    }

    public void IncreaseDamage(int amount)
    {
        playerDamageMax += amount;
        Debug.Log("Player Damage: " + playerDamageMax);
    }
}
