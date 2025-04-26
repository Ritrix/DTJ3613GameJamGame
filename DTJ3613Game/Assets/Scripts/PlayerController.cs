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

    bool isAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (lightAttackAction.IsPressed())
        {
            isAttacking = true;
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                move.x = 0.0f;
                move.y = 0.0f;
            }
        }
    }

    public void alertObservers(string message)
    {
        if (message == "LightAttackEnd")
            isAttacking = false;
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
