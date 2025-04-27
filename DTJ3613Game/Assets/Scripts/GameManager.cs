using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentWave = 1;
    public int playerGold = 0;

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
        //SceneManager.LoadScene("ShopScene");
    }
}
