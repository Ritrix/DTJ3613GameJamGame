using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Zealot : MonoBehaviour
{
    [Header("misc")]
    [SerializeField] private Animator m_Animator;
    public GameObject player;
    private Coroutine attackCoroutine;
    private bool isAttacking = false;
    private bool stun = false;
    public int baseGoldReward = 1;

    [Header("movement")]
    public float speed = 5.0f;
    private float distance;
    [SerializeField] private float aggroDistance = 7.0f;
    [SerializeField] private float separationDistance = 1.0f; // How far you want to stop away from player
    [SerializeField] private float reachThreshold = 0.05f; // a bit of leeway on the stop distance from the player
    public Vector2 minBounds;
    public Vector2 maxBounds;


    private const string HIT_PARAM = "isHit";
    private const string PLAYER_LIGHT1_HITBOX = "playerHitboxLight1";
    private const string PLAYER_MEDIUM_HITBOX = "PlayerHitboxMedium";

    [Header("Health")]
    public int maxHealth = 3;
    int currentHealth;

    private int plebNumber;

    [SerializeField] private GameObject floatingTextPrefab;

    private int damageModifier;

    [Header("Misc")]
    public AudioClip[] hurtSounds;
    public AudioSource audioSource;
    public GameObject projectilePrefab;

    [Header("Pitch Variation")]
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    public void PlaySound(string sound)
    {
        if (audioSource != null)
        {
            if (sound == "hurt" && hurtSounds.Length > 0)
            {
                int rand = Random.Range(0, hurtSounds.Length); // Pick random sound
                audioSource.pitch = Random.Range(minPitch, maxPitch);   // Randomize pitch
                audioSource.PlayOneShot(hurtSounds[rand]);
            }
        }
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    private void Start()
    {
        damageModifier = GameManager.Instance.playerCurrentMaxDamage;
        plebNumber = Random.Range(1, 1000);
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

            // After movement, clamp inside play area bounds
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
                transform.position.z
            );
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
            enemySpawner.enemiesDefeated++;
        }
        if (message == "hurtSound")
        {
            PlaySound("hurt");
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
        launch();


        yield return new WaitForSeconds(2.0f); // cooldown between attacks

        attackCoroutine = null; // set to null so ready to attack again
        isAttacking = false;
    }

    void launch()
    {
        // Fixing CS0034 by explicitly converting Vector2 to Vector3
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position + (Vector3)(Vector2.up * 0.5f), Quaternion.identity);
        ProjectileZealot projectile = projectileObject.GetComponent<ProjectileZealot>();
        // face towards player
        if (player.transform.position.x < transform.position.x)
        {
            // Player is to the left
            projectile.launch(Vector2.left, 300);
        }
        else
        {
            // Player is to the right
            projectile.launch(Vector2.right, 300);
        }
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PLAYER_LIGHT1_HITBOX && m_Animator.GetBool("dying") == false)
        {
            ShowFloatingText((1 + damageModifier).ToString(), Color.red, new Vector3(-1, 1, 0));
            changeHealth(-1);
        }
        else if (collision.gameObject.tag == PLAYER_MEDIUM_HITBOX && m_Animator.GetBool("dying") == false)
        {
            ShowFloatingText((3 + damageModifier).ToString(), Color.red, new Vector3(-1, 1, 0));
            changeHealth(-3);
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
            ComboSystem.Instance.RegisterHit();
        }

        currentHealth = Mathf.Clamp(currentHealth + amount - damageModifier, 0, maxHealth);
        UIHandler.instance.SetEnemyHealthValue(currentHealth / (float)maxHealth);
        UIHandler.instance.SetEnemiesNameLabelText("Zealot #" + plebNumber + ":");

        if (currentHealth <= 0)
        {
            int goldToGive = baseGoldReward * Mathf.Max(ComboSystem.Instance.GetCurrentCombo(), 1);
            ShowFloatingText("+" + goldToGive.ToString(), new Color(1f, 0.84f, 0f), new Vector3(1, 1, 0));
            GameManager.Instance.AddGold(goldToGive);
            stun = true;
            m_Animator.SetTrigger("isDead");
            m_Animator.SetBool("dying", true);
        }
    }
    /// <summary>
    /// Shows floating text above the enemy
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <param name="direction"></param>
    private void ShowFloatingText(string message, Color color, Vector3 direction)
    {
        Vector2 enemyPosition = transform.position;
        Vector2 spawnPosition = new Vector2(enemyPosition.x, enemyPosition.y + 4f);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(spawnPosition);
        GameObject textObj = Instantiate(floatingTextPrefab, screenPosition, Quaternion.identity, FindFirstObjectByType<Canvas>().transform);
        FloatingText floatingText = textObj.GetComponent<FloatingText>();
        floatingText.Initialize(message, color, direction);
    }
}
