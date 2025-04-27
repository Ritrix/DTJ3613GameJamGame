using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public void retry()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
