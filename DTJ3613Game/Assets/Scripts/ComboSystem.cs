using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance { get; private set; }

    [Header("Combo Settings")]
    public float comboResetTime = 1.0f; // how long you can go without hitting

    private int currentCombo = 0;
    private float comboTimer = 0f;

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
    }

    public void RegisterHit()
    {
        currentCombo++;
        comboTimer = comboResetTime;

        // OPTIONAL: Update combo UI here!
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

        // OPTIONAL: Update combo UI here!
        Debug.Log("Combo reset.");
    }
}
