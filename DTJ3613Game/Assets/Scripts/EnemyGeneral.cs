using System.Collections;
using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{
    [Header("misc")]
    [SerializeField] private Animator m_Animator;
    public GameObject player;
    private Coroutine attackCoroutine;

    [Header("movement")]
    public float speed = 5.0f;
    private float distance;
    [SerializeField] private float aggroDistance = 7.0f;
    [SerializeField] private float separationDistance = 1.0f; // How far you want to stop away from player

    private const string HIT_PARAM = "isHit";
    private const string PLAYER_LIGHT1_HITBOX = "playerHitboxLight1";

    private void Update()
    {
        facePlayer();

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < aggroDistance)
        {
            if (distance > separationDistance + 0.5f) // If still a bit far, move normally
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else
            {
                // align Y axis
                Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

                

                if (transform.position.x < player.transform.position.x)
                {
                    // Enemy is to the left, stop slightly to the left
                    targetPosition.x -= separationDistance;
                }
                else
                {
                    // Enemy is to the right, stop slightly to the right
                    targetPosition.x += separationDistance;
                }

                // move towards for an attack
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // start a coroutine to attack after a moment
                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(AttackAfterDelay());
                }
            }
        }
        else
        {
            // If player moves out of aggro range, stop attacking
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }

    }

    private IEnumerator AttackAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        m_Animator.SetTrigger("isAttacking");

        
        yield return new WaitForSeconds(1.0f); // cooldown between attacks

        attackCoroutine = null; // set to null so ready to attack again
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == PLAYER_LIGHT1_HITBOX)
        {
            m_Animator.SetTrigger(HIT_PARAM);
        }
    }

    private void facePlayer()
    {
        // face towards player
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
    }
}
