using UnityEngine;
using UnityEngine.SceneManagement;

public class backStoryController : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
