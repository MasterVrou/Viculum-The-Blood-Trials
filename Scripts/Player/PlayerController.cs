using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private float movementInputDirection;
    private float knockbackStartTime;
    private float dodgeStartTime;

    [SerializeField]
    private float knockbackDuration;
    [SerializeField]
    private float dodgeDuration;

    private bool isFacingRight = true;
    private bool isRunning = false;
    private bool isGrounded = false;
    private bool isTouchingWall = false;
    private bool canJump = false;
    private bool isTouchingLedge = false;
    private bool canClimbLedge = false;
    private bool ledgeDetected = false;
    private bool canMove = true;
    private bool canFlip = true;
    private bool knockback;
    private bool dodge;

    [SerializeField]
    private Vector2 knockbackSpeed;
    [SerializeField]
    private Vector2 dodgeSpeed;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;

    public float movementSpeed = 10;
    public float jumpForce = 16;

    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public float wallCheckDistance;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //knockbackSpeed.x = 2;
        //knockbackSpeed.y = 2;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        UpdateAnimations();
        CheckLedgeClimb();
        CheckKnockback();
        CheckDodge();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        //To climb the player needs to touch the wall with the lower raycast(isTouchingWall) and not touch the wall with the upper raycast
        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if(isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
                
            }

            canMove = false;
            canFlip = false;

            if (canClimbLedge)
            {
                transform.position = ledgePos1;
            }
            anim.SetBool("canClimbLedge", canClimbLedge);
        }
    }

    private void CheckDodge()
    {
        if(Time.time >= dodgeStartTime + dodgeDuration  && dodge)
        {
            dodge = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void Dodge()
    {
        dodge = true;
        dodgeStartTime = Time.time;

        if (isGrounded)
        {
            if (isFacingRight)
            {
                rb.velocity = new Vector2(-1 * dodgeSpeed.x, dodgeSpeed.y);
            }
            else
            {
                rb.velocity = new Vector2(dodgeSpeed.x, dodgeSpeed.y);
            }

        }
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    public void KnockBack(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }

    public int GetFacingDirection()
    {
        if (isFacingRight)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight &&  movementInputDirection < 0)
        {
            FlipSprite();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            FlipSprite();
        }

        if(Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);

    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            Dodge();
        }

    }

    private void Jump()
    {
        if(canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void ApplyMovement()
    {
        //themataki

        if (canMove && !knockback && !dodge)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        else if(!knockback && !dodge)
        {
            rb.velocity = new Vector2(0, 0);
        }

    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    private void FlipSprite()
    {
        if(canFlip && !knockback && !dodge)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
}
