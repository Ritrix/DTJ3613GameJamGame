using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    private VisualElement m_HealthBar;
    private VisualElement m_EnemyHealthBar;
    private Label m_waveLabel;
    public static UIHandler instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_HealthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        m_EnemyHealthBar = uiDocument.rootVisualElement.Q<VisualElement>("EnemyHealthBar");
        
        SetHealthValue(1.0f);
        SetEnemyHealthValue(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealthValue(float percentage)
    {
        m_HealthBar.style.width = Length.Percent(100 * percentage);
    }

    public void SetEnemyHealthValue(float percentage)
    {
        m_EnemyHealthBar.style.width = Length.Percent(100 * percentage);
    }

    public void SetWaveLabelText(string text)
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_waveLabel = uiDocument.rootVisualElement.Q<Label>("WaveLabel");
        m_waveLabel.text = text;
    }

    public void SetEnemiesRemainingLabelText(string text)
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_waveLabel = uiDocument.rootVisualElement.Q<Label>("enemiesRemainingLabel");
        m_waveLabel.text = text;
    }
}
