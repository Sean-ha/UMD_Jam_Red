using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private LayerMask whatIsGround;

    private Transform overlapBoxTransform;
    private GameObject interactable;

    private bool isFacingRight = true;

    private float movementSpeed = 6;
    private float jumpForce = 20;
    private float jumpLowForce = 10;
    private float horizontal;

    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        whatIsGround = LayerMask.GetMask("Ground");
        overlapBoxTransform = transform.Find("OverlapBoxTransform");
    }

    void Update()
    {
        CheckInteract();
        if (canMove)
        {
            CheckMovement();
            CheckJump();
            CheckCancelJump();
        }
        else
        {

        }
        animator.SetFloat("velocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("horizontalVelocity", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * movementSpeed, rb.velocity.y);
    }

    private void CheckInteract()
    {
        if (interactable != null)
        {
            // Press up to interact
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                interactable.GetComponent<DialogueTrigger>().TriggerDialogue();
                canMove = false;
            }
        }
    }

    #region movement

    private void CheckMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal < 0 && isFacingRight)
        {
            isFacingRight = false;
            animator.Play("Player_Turn");
        }
        else if (horizontal > 0 && !isFacingRight)
        {
            isFacingRight = true;
            animator.Play("Player_Turn");
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void CheckCancelJump()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.velocity.y > jumpLowForce)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpLowForce);
            }
        }
    }

    private bool IsGrounded()
    {
        Collider2D overlap = Physics2D.OverlapBox(new Vector2(overlapBoxTransform.position.x, overlapBoxTransform.position.y),
                            new Vector2(0.52f, 0.02f), 0f, whatIsGround);

        if (overlap != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    // For visualizing ground checker box thing
    /*    void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector2(overlapBoxTransform.position.x, overlapBoxTransform.position.y),
                                new Vector2(0.52f, 0.02f));
        }*/

    // Called when the turn around animation finishes
    public void TurnAround()
    {
        if (isFacingRight)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            interactable = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            interactable = null;
        }
    }
}
