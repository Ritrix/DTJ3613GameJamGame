using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{

    [SerializeField] private Animator m_Animator;
    public GameObject player;
    public float speed = 5.0f;

    private float distance;

    private const string HIT_PARAM = "isHit";
    private const string PLAYER_LIGHT1_HITBOX = "playerHitboxLight1";

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Compare X positions
        if (player.transform.position.x < transform.position.x)
        {
            // Player is to the left
            transform.localScale = new Vector3(-1f, 1f, 1f); // Flip X scale
        }
        else
        {
            // Player is to the right
            transform.localScale = new Vector3(1f, 1f, 1f); // Normal scale
        }

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == PLAYER_LIGHT1_HITBOX)
        {
            m_Animator.SetTrigger(HIT_PARAM);
        }
    }
}
