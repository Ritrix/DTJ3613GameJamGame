using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance { get; private set; }

    [Header("Combo Settings")]
    public float comboResetTime = 1.0f; // how long you can go without hitting

    private int currentCombo = 0;
    private float comboTimer = 0f;

    [SerializeField] private ComboUI comboUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (currentCombo > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
        UIHandler.instance.SetGoldLabelText("Gold: " + GameManager.Instance.playerGold);
    }

    public void RegisterHit()
    {
        currentCombo++;
        comboTimer = comboResetTime;

        if (comboUI != null)
            comboUI.UpdateCombo(currentCombo);

        Debug.Log($"Combo: {currentCombo}");
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        comboTimer = 0f;

        if (comboUI != null)
            comboUI.UpdateCombo(0);

        Debug.Log("Combo reset.");
    }
}
