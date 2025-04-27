using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public TMP_Text m_TextComponent;
    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        m_TextComponent.text = "Game Over\nWave: " + GameManager.Instance.currentWave;
    }

    public void retry()
    {
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("mainMenu");
    }
}
