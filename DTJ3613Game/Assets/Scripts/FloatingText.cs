using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float lifeTime = 1f;
    private Vector3 moveDirection;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Initialize(string message, Color color, Vector3 direction)
    {
        text.text = message;
        text.color = color;
        moveDirection = direction.normalized;
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }
}
