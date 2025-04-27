using UnityEngine;

public class ProjectileZealot : MonoBehaviour
{
    Rigidbody2D rigidbody2d;


    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);

        if (direction.x > 0)
        {
            // Facing right
            transform.localScale = new Vector3(1f, 1f, 1f); // Reset scale for right direction
        }
        else if (direction.x < 0)
        {
            // Facing left
            transform.localScale = new Vector3(-1f, 1f, 1f); // Flip scale for left direction
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }
}
