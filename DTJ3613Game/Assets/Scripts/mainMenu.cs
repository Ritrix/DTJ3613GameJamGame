using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void startGame()
    {
        SceneManager.LoadScene("BackStory");
    }

    public void tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
