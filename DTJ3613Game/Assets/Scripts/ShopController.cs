using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    public void NextWave()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
