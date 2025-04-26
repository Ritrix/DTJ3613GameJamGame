using UnityEngine;
using UnityEngine.InputSystem;

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

    bool isAttacking;
    int typeAttack; // type of attack: Not attacking = 0, Light = 1, medium = 2, special = 3

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        moveAction.Enable();
        lightAttackAction.Enable();
        playerRigidBody = GetComponent<Rigidbody2D>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        // set move vector to current input
        move = moveAction.ReadValue<Vector2>();

        // stop moving when light attack pressed
        if (lightAttackAction.IsPressed() && typeAttack < 1)
        {
            animator.SetTrigger("isLAttackingSTrigger");
            isAttacking = true;
            typeAttack = 1;
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                move.x = 0.0f;
                move.y = 0.0f;
            }
        }
    }

    /// <summary>
    /// alert method to be used when the animation for an attack by the player has ended. ensure to update this when attacks are added, and to reference this at the end of an animation by adding an animation event to the animations (maybe edit this for combos)
    /// </summary>
    /// <param name="message"></param>
    public void alertObservers(string message)
    {
        if (message == "LightAttackEnd")
        {
            isAttacking = false;
            typeAttack = 0;
        }
            

    }

    private void FixedUpdate()
    {

        
        // move player character
        if (!isAttacking)
        {
            Vector2 position = (Vector2)playerRigidBody.position + move * runSpeed * Time.deltaTime;
            playerRigidBody.MovePosition(position);
        }
    }


}
