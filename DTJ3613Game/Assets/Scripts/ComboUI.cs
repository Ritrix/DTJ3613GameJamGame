using UnityEngine;
using TMPro;

public class ComboUI : MonoBehaviour
{
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private float shakeAmount = 5f;
    [SerializeField] private float scaleMultiplier = 0.1f;
    [SerializeField] private float scaleSpeed = 5f;

    private Vector3 originalPosition;
    private Vector3 targetScale = Vector3.one;

    private void Awake()
    {
        if (comboText == null)
            comboText = GetComponent<TMP_Text>();

        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Smoothly scale back to normal size
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void UpdateCombo(int comboAmount)
    {
        if (comboAmount <= 0)
        {
            comboText.text = "";
            return;
        }

        comboText.text = "Combo x" + comboAmount;

        // Shake
        Vector3 randomShake = Random.insideUnitCircle * shakeAmount;
        transform.localPosition = originalPosition + randomShake;

        // Scale up a little based on combo
        float extraScale = 1f + (comboAmount * scaleMultiplier);
        transform.localScale = Vector3.one * extraScale;
    }
}
