using System.Collections;
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
    public bool mediumAttack = false;

    [Header("Attacks")]
    public InputAction lightAttackAction;
    public InputAction MediumAttackAction;
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

    public int currentComnbo = 0; // current combo stage

    private bool stun;
    private bool canAttack = true; // can attack or not



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        maxHealth = GameManager.Instance.playerCurrentMaxHealth;
        runSpeed = GameManager.Instance.playerCurrentMaxSpeed;
        originalPlayerScale = playerVisual.transform.localScale;
        animator = GetComponent<Animator>();
        moveAction.Enable();
        lightAttackAction.Enable();
        MediumAttackAction.Enable();
        playerRigidBody = GetComponent<Rigidbody2D>();
        isAttacking = false;
        currentHealth = maxHealth;
        animator.SetBool("dying", false);
        currentComnbo = 0;
        canAttack = true;

    }

    // Update is called once per frame
    void Update()
    {
        // set move vector to current input
        move = moveAction.ReadValue<Vector2>();

        // function to handle all movement related items and controls
        movement();


        // function to handle all attack related items and controls
        if (canAttack)
        {
            attacks();
        }



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
            canAttack = false;
            isAttacking = false;
            typeAttack = 0;
            StartCoroutine(attackEndLag(0.1f));
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
        else if (message == "MediumAttackFend")
        {
            canAttack = false;
            isAttacking = false;
            typeAttack = 0;
            animator.SetInteger("mediumAttackType", 0);
            StartCoroutine(attackEndLag(0.5f));
        }
        else if (message == "MediumAttackNend")
        {
            canAttack = false;
            isAttacking = false;
            typeAttack = 0;
            animator.SetInteger("mediumAttackType", 0);
            StartCoroutine(attackEndLag(0.4f));
        }
        else if (message == "MediumAttackBend")
        {
            canAttack = false;
            isAttacking = false;
            typeAttack = 0;
            animator.SetInteger("mediumAttackType", 0);
            StartCoroutine(attackEndLag(0.2f));
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
            changeHealth(-1);
            animator.SetTrigger("playerHurt");
        }
    }

    #region private custom functions

    private void movement()
    {
        // if moving set move animation to true
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
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
            if (MediumAttackAction.IsPressed() && typeAttack < 2)
            {
                isAttacking = true;
                typeAttack = 2;

                if (!Mathf.Approximately(move.x, 0.0f))
                {
                    if (playerVisual.transform.localScale == originalPlayerScale) // is facing right
                    {

                        //1 = lunge, 2 = back attack, 3 = neutral attack
                        if (move.x > 0.0f) // do lunge
                        {
                            animator.SetInteger("mediumAttackType", 1);
                            CheckAndSetStationary();
                            StartCoroutine(Lunge(transform.right, 5.0f, 0.2f));

                        }
                        else if (move.x < 0.0f) // do back attack
                        {
                            animator.SetInteger("mediumAttackType", 2);
                            CheckAndSetStationary();
                        }
                    }
                    else // is facing left
                    {
                        if (move.x > 0.0f) // do back attack
                        {
                            animator.SetInteger("mediumAttackType", 2);
                            CheckAndSetStationary();
                        }
                        else if (move.x < 0.0f) // do lunge
                        {
                            animator.SetInteger("mediumAttackType", 1);
                            CheckAndSetStationary();
                            StartCoroutine(Lunge(transform.right, -5.0f, 0.2f));
                        }
                    }
                }
                else // do neutral attack
                {
                    animator.SetInteger("mediumAttackType", 3);
                    CheckAndSetStationary();
                }




            }
            if (lightAttackAction.IsPressed() && typeAttack < 1)
            {
                isAttacking = true;
                animator.SetBool("continueLightAttack", true);
            }
            // stop moving when light attack pressed
            if (lightAttackAction.IsPressed() && typeAttack < 1)
            {
                animator.SetTrigger("isLAttackingSTrigger");
                typeAttack = 1;
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
        if (MediumAttackAction.WasReleasedThisFrame())
        {
            animator.SetInteger("mediumAttackType", 0);
        }
    }

    private void CheckAndSetStationary()
    {
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            move.x = 0.0f;
            move.y = 0.0f;
        }
    }
    #endregion


    public IEnumerator attackEndLag(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    public IEnumerator Lunge(Vector2 direction, float distance, float duration)
    {
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + (direction.normalized * distance);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap exactly to target at the end (optional)
        transform.position = targetPosition;
    }
}
