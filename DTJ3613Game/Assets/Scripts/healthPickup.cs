using UnityEngine;

public class healthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.changeHealth(1);
            Destroy(gameObject);
        }
    }
}
