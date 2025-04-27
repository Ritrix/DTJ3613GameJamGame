using System.Collections;
using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{
    [Header("misc")]
    [SerializeField] private Animator m_Animator;
    public GameObject player;
    private Coroutine attackCoroutine;
    private bool isAttacking = false;
    private bool stun = false;

    [Header("movement")]
    public float speed = 5.0f;
    private float distance;
    [SerializeField] private float aggroDistance = 7.0f;
    [SerializeField] private float separationDistance = 1.0f; // How far you want to stop away from player
    [SerializeField] private float reachThreshold = 0.05f; // a bit of leeway on the stop distance from the player


    private const string HIT_PARAM = "isHit";
    private const string PLAYER_LIGHT1_HITBOX = "playerHitboxLight1";

    [Header("Health")]
    public int maxHealth = 3;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        m_Animator.SetBool("dying", false);
    }

    private void Update()
    {
        facePlayer();

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < aggroDistance && !stun)
        {
            if (distance > separationDistance + 0.5f && !isAttacking) // If still a bit far, move normally
            {
                m_Animator.SetBool("isWalking", true);
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

                if (!isAttacking)
                {
                    // move towards for an attack
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                }

                float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

                if (distanceToTarget <= reachThreshold && attackCoroutine == null)
                {
                    m_Animator.SetBool("isWalking", false);
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

    public void alertObservers(string message)
    {
        if (message == "stunEnd")
        {
            stun = false;
        }
        if (message == "deleteEnemy")
        {
            Destroy(gameObject);
        }


    }

    private IEnumerator AttackAfterDelay()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);

        if (!stun)
        {
            m_Animator.SetTrigger("isAttacking");
        }
        

        
        yield return new WaitForSeconds(1.0f); // cooldown between attacks

        attackCoroutine = null; // set to null so ready to attack again
        isAttacking = false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == PLAYER_LIGHT1_HITBOX && m_Animator.GetBool("dying") == false)
        {
            changeHealth(-1);
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


    /// <summary>
    /// changes player character health
    /// </summary>
    /// <param name="amount"></param>
    public void changeHealth(int amount)
    {
        if (amount < 0)
        {
            stun = true;
            m_Animator.SetTrigger(HIT_PARAM);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetEnemyHealthValue(currentHealth / (float)maxHealth);

        if (currentHealth <= 0)
        {
            stun = true;
            m_Animator.SetTrigger("isDead");
            m_Animator.SetBool("dying", true);
        }
    }
}
