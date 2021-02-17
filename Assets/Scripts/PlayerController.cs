using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private ParticleSystem walkDust;
    private ParticleSystem.ShapeModule dustShape;
    private ParticleSystem jumpParticles;
    private SoundManager sm;
    private GameData gd;
    private WateringCan wateringCan;

    private LayerMask whatIsGround;

    private Transform overlapBoxTransform;
    private GameObject interactable;
    private Image blackTransition;

    private bool isFacingRight = true;

    private float movementSpeed = 6;
    private float jumpForce = 20;
    private float jumpLowForce = 10;
    private float horizontal;

    private bool canMove = true;
    private bool isStatic;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        whatIsGround = LayerMask.GetMask("Ground");
        overlapBoxTransform = transform.Find("OverlapBoxTransform");
        walkDust = transform.Find("WalkDust").GetComponent<ParticleSystem>();
        dustShape = walkDust.shape;
        jumpParticles = transform.Find("JumpParticles").GetComponent<ParticleSystem>();
        wateringCan = transform.Find("WateringCan").GetComponent<WateringCan>();
        blackTransition = GameObject.FindGameObjectWithTag("BlackTransition").GetComponent<Image>();
    }

    private void Start()
    {
        sm = SoundManager.instance;
        gd = GameData.instance;
    }

    void Update()
    {
        // REMOVE LATER; DEBUGGING PURPOSES ONLY
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 5;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1;
        }

        if (canMove)
        {
            CheckMovement();
            CheckJump();
            CheckCancelJump();
            CheckInteract();
            CheckWateringCan();
        }

        if (isFacingRight)
        {
            dustShape.rotation = new Vector3(0, 0, -200);
        }
        else
        {
            dustShape.rotation = new Vector3(0, 0, -2);
        }

        animator.SetFloat("velocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("horizontalVelocity", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        if (!isStatic)
        {
            rb.velocity = new Vector2(horizontal * movementSpeed, rb.velocity.y);
        }
    }

    #region Interaction

    private void CheckInteract()
    {
        if (interactable != null && IsGrounded())
        {
            // Press up to interact
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                // Different interactable objects
                if (interactable.CompareTag("NPC"))
                {
                    if (interactable.transform.localPosition.x < transform.localPosition.x)
                    {
                        isFacingRight = false;
                        transform.localScale = new Vector2(-1, 1);
                    }
                    else
                    {
                        isFacingRight = true;
                        transform.localScale = new Vector2(1, 1);
                    }

                    interactable.GetComponent<DialogueTrigger>().TriggerDialogue();
                    animator.Play("Player_LookUp");
                    canMove = false;
                    horizontal = 0;
                }
                else if (interactable.CompareTag("Door"))
                {
                    canMove = false;
                    horizontal = 0;
                    interactable.GetComponent<Door>().EnterDoor();
                }
            }
        }
    }

    private void CheckWateringCan()
    {
        if (Input.GetKeyDown(KeyCode.Z) && IsGrounded())
        {
            if (gd.gotWaterCan)
            {
                WaterGround();
            }
        }
    }

    private void WaterGround()
    {
        canMove = false;
        horizontal = 0;
        wateringCan.gameObject.SetActive(true);

        if (isFacingRight)
        {
            ParticleSystem.ShapeModule water = wateringCan.waterParticles.shape;
            water.rotation = new Vector3(0, 0, -105);
        }
        else
        {
            ParticleSystem.ShapeModule water = wateringCan.waterParticles.shape;
            water.rotation = new Vector3(0, 0, -210);
        }
        
        wateringCan.GetComponent<Animator>().Play("WateringCan_Water");
    }

    private void Die()
    {
        sm.PlaySound(SoundManager.Sound.Die);
        rb.bodyType = RigidbodyType2D.Static;
        isStatic = true;
        canMove = false;
        horizontal = 0;
        animator.Play("Player_Death");
        CameraShake.instance.ShakeCamera(6, 0.5f);
    }

    #endregion

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
        sm.PlaySound(SoundManager.Sound.Jump);
        jumpParticles.Play();
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

    public void EmitWalkingParticles()
    {
        walkDust.Play();
    }

    #endregion

    #region Animation Events

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

    public void SetFacingLeft()
    {
        isFacingRight = false;
        transform.localScale = new Vector2(-1, 1);
    }

    public void SetCanMove(bool to)
    {
        canMove = to;

        // QOL player input not getting eaten
        if (canMove)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector2(-1, 1);
                isFacingRight = false;
                horizontal = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.localScale = new Vector2(1, 1);
                isFacingRight = true;
                horizontal = 1;
            }
        }
    }

    public void PlayWalkSound()
    {
        sm.GetAudioSource(SoundManager.Sound.Walk).pitch = Random.Range(.95f, 1);
        sm.PlaySound(SoundManager.Sound.Walk);
    }

    public void PlayPoofSound()
    {
        sm.PlaySound(SoundManager.Sound.Poof);
    }


    // Spawns at the most recent checkpoint (flower)
    public void Respawn()
    {
        int sceneToSpawnAt = 0;
        // If no flowers have been watered yet, auto respawn at start of 6th scene
        if (gd.respawnScene == 0)
        {
            sceneToSpawnAt = 6;
        }
        else
        {
            sceneToSpawnAt = gd.respawnScene;
        }
        SceneLoader.instance.SetSceneLoadData(sceneToSpawnAt, gd.respawnPosition, false);

        blackTransition.color = new Color(0, 0, 0, 1);
        blackTransition.rectTransform.anchoredPosition = new Vector2(2000, 0);

        LeanTween.move(blackTransition.rectTransform, new Vector2(0, 0), .15f).setOnComplete(TransitionScene);
    }

    private void TransitionScene()
    {
        int sceneToSpawnAt = 0;
        // If no flowers have been watered yet, auto respawn at start of 6th scene
        if (gd.respawnScene == 0)
        {
            sceneToSpawnAt = 6;
        }
        else
        {
            sceneToSpawnAt = gd.respawnScene;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToSpawnAt);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") || collision.CompareTag("Door"))
        {
            interactable = collision.gameObject;
        }
        else if (collision.CompareTag("SceneLoader"))
        {
            canMove = false;
            horizontal = 0;
            collision.GetComponent<Door>().EnterDoor();
        }
        else if (collision.CompareTag("AutoNPC"))
        {
            collision.GetComponent<DialogueTrigger>().TriggerDialogue();
            canMove = false;
            horizontal = 0;
        }
        else if (collision.CompareTag("Death"))
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") || collision.CompareTag("Door"))
        {
            interactable = null;
        }
    }
}
