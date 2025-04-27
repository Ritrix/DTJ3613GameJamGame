using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 1.0f;
    public InputAction moveAction;
    Rigidbody2D playerRigidBody;
    Vector2 move;


    [Header("Attacks")]
    public InputAction lightAttackAction;
    [SerializeField] private Animator animator;
    public bool isAttacking;
    int typeAttack; // type of attack: Not attacking = 0, Light = 1, medium = 2, special = 3
    [SerializeField] private Transform playerVisual;
    private Vector3 originalPlayerScale; // keeping track of the original player scale as we flip him around

    [Header("Health")]
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }
    // damage cooldown
    public float timeInvincible = 1.0f;
    bool isInvincible;
    float damageCooldown;


    public bool blocking;
    private bool stun;

    private int lightAttackStage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPlayerScale = playerVisual.transform.localScale;
        animator = GetComponent<Animator>();
        moveAction.Enable();
        lightAttackAction.Enable();
        playerRigidBody = GetComponent<Rigidbody2D>();
        isAttacking = false;
        currentHealth = maxHealth;
        blocking = false;
        lightAttackStage = 0;
        animator.SetBool("dying", false);
    }

    // Update is called once per frame
    void Update()
    {
        // set move vector to current input
        move = moveAction.ReadValue<Vector2>();

        // function to handle all movement related items and controls
        movement();

        // function to handle all attack related items and controls
        attacks();

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
    }

    /// <summary>
    /// alert method to be used when the animation for an attack by the player has ended. ensure to update this when attacks are added, and to reference this at the end of an animation by adding an animation event to the animations (maybe edit this for combos)
    /// </summary>
    /// <param name="message"> accepts a message from the animation to trigger something here </param>
    public void alertObservers(string message)
    {
        if (message == "LightAttackEnd")
        {
            isAttacking = false;
            typeAttack = 0;
        }
        else if(message == "stunEnd")
        {
            isAttacking = false;
            typeAttack = 0;
            stun = false;
        }
        else if (message == "PlayerDeath")
        {
            Debug.Log("Player died!");
            SceneManager.LoadScene("gameOver");
        }


    }

    private void FixedUpdate()
    {
        // move player character
        if (!isAttacking && !stun)
        {
            Vector2 position = (Vector2)playerRigidBody.position + move * runSpeed * Time.deltaTime;
            playerRigidBody.MovePosition(position);
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
            if (isInvincible)
            {
                return;
            }
            animator.SetTrigger("playerHurt");
            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

        if(currentHealth <= 0)
        {
            animator.SetTrigger("isDead");
            animator.SetBool("dying", true); 
        }
    }

    /// <summary>
    /// check if player is hit
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "basicEnemyAttackHitbox" && animator.GetBool("dying") == false)
        {
            if (!blocking)
            {
                changeHealth(-1);
                animator.SetTrigger("playerHurt");
            }
            else
            {

            }
        }
    }

    #region private custom functions

    private void movement()
    {
        // if moving set move animation to true
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            animator.SetBool("isWalking", true);
            blocking = false;
        }
        else
        {
            animator.SetBool("isWalking", false);
            if (!isAttacking)
            {
                blocking = true;
            }
        }

        // change player direction
        if (!isAttacking)
        {
            if (move.x > 0.0f)
            {
                playerVisual.transform.localScale = originalPlayerScale; // have player face right
            }
            else if (move.x < 0.0f)
            {
                playerVisual.transform.localScale = new Vector3(-originalPlayerScale.x, originalPlayerScale.y, originalPlayerScale.z);
            }
        }
    }

    private void attacks()
    {
        if (!stun)
        {
            if (lightAttackAction.IsPressed() && typeAttack < 1)
            {
                isAttacking = true;
                blocking = false;
                animator.SetBool("continueLightAttack", true);
            }
            // stop moving when light attack pressed
            if (lightAttackAction.IsPressed() && typeAttack < 1)
            {
                animator.SetTrigger("isLAttackingSTrigger");
                isAttacking = true;
                blocking = false;
                typeAttack = 1;
                lightAttackStage = 1;
                if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
                {
                    move.x = 0.0f;
                    move.y = 0.0f;
                }
            }
            
        }

        if (lightAttackAction.WasReleasedThisFrame())
        {
            animator.SetBool("continueLightAttack", false);
        }
    }

    #endregion

}
