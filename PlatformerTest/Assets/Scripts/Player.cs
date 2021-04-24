using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 velocity;
    private Vector2 backupVelocity;
    private bool applyBackup;
    private Rigidbody2D body;
    private BoxCollider2D playerCollider;
    private int horizontal;
    private int direction;
    private int jumpFrames;
    private int wallJumpFrames;
    private int dashFrames;
    private bool isGrounded;
    private bool isWalledL;
    private bool isWalledR;
    private int wallJumpDir;
    private bool jumpInputUsed;
    private int framesSinceGround;
    private bool jumpButtonActive;
    private bool doubleJumpActive;
    private bool dashButtonActive;
    private bool dashFromGround;
    private float checkBoxOffset = 0.05f;
    private bool unlimitedJump;

    private static bool downPressed;
    private static bool upPressed;
    private static bool leftPressed;
    private static bool rightPressed;
    private static bool dashPressed;
    private static bool jumpPressed;

    [SerializeField]
    private LayerMask platformLayer;

    [Header("Horizontal movement")]
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private int accFrames;
    [SerializeField]
    private int decFrames;

    [Header("Vertical movement")]
    [SerializeField]
    private float maxFallSpeed;
    [SerializeField]
    private float maxQuickFallSpeed;
    [SerializeField]
    private float decVert;
    [SerializeField]
    private float accVert;

    [Header("Jump movement")]
    [SerializeField]
    private float maxRizeSpeed;
    [SerializeField]
    private int maxJumpFrames;
    [SerializeField]
    private int maxFloatJumpFrames;

    [Header("Double Jump movement")]
    [SerializeField]
    private float doubleJumpSpeed;
    [SerializeField]
    private bool doubleJumpEnabled;

    [Header("Wall Jump movement")]
    [SerializeField]
    private float maxWallJumpFrames;
    [SerializeField]
    private float wallJumpFirstFrames;
    [SerializeField]
    private bool wallRestoresDoubleJump;
    [SerializeField]
    private bool wallRestoresDash;
    [SerializeField]
    private bool wallJumpEnabled;

    [Header("Dash movement")]
    [SerializeField]
    private float maxDashSpeed;
    [SerializeField]
    private int maxDashFrames;
    [SerializeField]
    private int dashCooldownFrames;

    [Header("Unussual movement")]
    [SerializeField]
    private bool midJumpDash;
    [SerializeField]
    private bool groundDashJump;
    [SerializeField]
    private float orbDashSpeedCoefficientX;
    [SerializeField]
    private float orbDashSpeedCoefficientY;
    [SerializeField]
    private float orbDashCurrentSpeedCoefficient;
    [SerializeField]
    private int orbDashRefreshTiming;
    [SerializeField]
    private float minOrbBounceMagnitude;


    private float eps;


    void Start()
    {
        Storage.LoadKeyBinds();
        Timer.Reset();
        body = gameObject.GetComponent<Rigidbody2D>();
        playerCollider = gameObject.GetComponent<BoxCollider2D>();
        velocity = new Vector2(0.0f, 0.0f);
        horizontal = 0;
        direction = 1;
        jumpPressed = false;
        dashPressed = false;
        jumpFrames = maxJumpFrames;
        dashFrames = maxDashFrames;
        jumpButtonActive = false;
        doubleJumpActive = false;
        dashButtonActive = false;
        dashFromGround = false;
        applyBackup = false;
        unlimitedJump = false;
        eps = 0.001f * maxSpeed / (accFrames + decFrames);
        Input.ResetInputAxes();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (CameraPosition.cameraTransition)
        {
            if (!applyBackup)
            {
                applyBackup = true;
                backupVelocity = body.velocity;
                body.velocity = Vector2.zero;
            }
            return;
        }

        if (applyBackup)
        {
            body.velocity = backupVelocity;
            applyBackup = false;
        }

        if (PauseMenu.gamePaused)
        {
            body.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
            velocity.y = Mathf.Max(velocity.y, 0.0f);
        else if (Mathf.Abs(velocity.y - body.velocity.y) > eps && jumpFrames > 0)
            jumpFrames = maxJumpFrames;
        velocity = body.velocity;

        isGroundedUpdate();
        isWalledRUpdate();
        isWalledLUpdate();

        if (isGrounded)
            unlimitedJump = false;

        horizontal = 0;
        if (Input.GetKey(KeyBinds.keyBinds["Right"]))
            ++horizontal;
        if (Input.GetKey(KeyBinds.keyBinds["Left"]))
            --horizontal;
        jumpPressed = Input.GetKey(KeyBinds.keyBinds["Jump"]);
        downPressed = Input.GetKey(KeyBinds.keyBinds["Down"]);
        dashPressed = Input.GetKey(KeyBinds.keyBinds["Dash"]);

        if (!jumpPressed)
            jumpInputUsed = false;
       
        if (horizontal != 0 && (dashFrames <= 0 || dashFrames >= maxDashFrames))
            direction = horizontal;
                       

        if (horizontal == 0)
        {
            if (velocity.x > 0)
                velocity.x = Mathf.Max(velocity.x - maxSpeed / decFrames, 0);
            else
                velocity.x = Mathf.Min(velocity.x + maxSpeed / accFrames, 0);
        }
        else
        {
            if (Mathf.Abs(velocity.x) < maxSpeed)
                unlimitedJump = false;

            if (velocity.x * horizontal < 0.0f)
                velocity.x += maxSpeed * horizontal / decFrames;
            else if (Mathf.Abs(velocity.x) < maxSpeed)
                velocity.x += maxSpeed * horizontal / accFrames;
        }

        if (!unlimitedJump)
        {
            if (velocity.x > maxSpeed)
                velocity.x = maxSpeed;
            if (velocity.x < -maxSpeed)
                velocity.x = -maxSpeed;
        }

        if (wallRestoresDoubleJump && (isWalledL || isWalledR))
            doubleJumpActive = true;
        if (isGrounded)
        {
            framesSinceGround = 0;
            doubleJumpActive = true;
        }
        else
            ++framesSinceGround;

        if (!jumpPressed)
            jumpButtonActive = true;

        if (isGrounded && jumpFrames == maxJumpFrames)
            jumpFrames = 0;

        if (JumpOrb.readyToJump && JumpOrb.instance != null && jumpFrames == maxJumpFrames)
            jumpFrames = 0;


        if (jumpPressed && jumpFrames < maxJumpFrames && (dashFrames <= 0 || dashFrames >= maxDashFrames) && (jumpButtonActive || jumpFrames > 0))
        {
            velocity.y = maxRizeSpeed;
            ++jumpFrames;
            jumpButtonActive = false;
            jumpInputUsed = true;

            if (JumpOrb.instance != null && jumpFrames == 1)
            {
                JumpOrb.instance.CooldownStart();
                dashFrames = 0;
            }
        }
        else if (dashFrames <= 0 || dashFrames >= maxDashFrames)
        {
            velocity.y = Mathf.Max(velocity.y - ((velocity.y > 0) ? decVert : accVert), -((downPressed) ? maxQuickFallSpeed : maxFallSpeed));

            if (jumpButtonActive && doubleJumpActive && jumpPressed && !isWalledR && !isWalledL && !jumpInputUsed && doubleJumpEnabled)
            {
                velocity.y = doubleJumpSpeed;
                doubleJumpActive = false;
                jumpButtonActive = false;
                jumpInputUsed = true;
            }

            if (isGrounded)
                jumpFrames = 0;
            else if ((jumpFrames != 0 || framesSinceGround > maxFloatJumpFrames) && (dashFrames <= 0 || dashFrames >= maxDashFrames))
            {
                jumpFrames = maxJumpFrames;
            }
        }
        else 
        { 
            if (jumpFrames > 0 && !midJumpDash)
                jumpFrames = maxJumpFrames;
            if (!isGrounded && jumpFrames == 0 && !groundDashJump)
                jumpFrames = maxJumpFrames;
        }

        if (!jumpInputUsed && jumpPressed && wallJumpEnabled)
        {
            if (isWalledR || isWalledL)
            {
                wallJumpDir = (isWalledL? -1 : 0) + (isWalledR? 1 : 0);
                wallJumpFrames = 1;
                jumpInputUsed = true;
                velocity.y = maxRizeSpeed;
                velocity.x = -wallJumpDir * maxSpeed;
            }                
        }
        else if (wallJumpFrames < maxWallJumpFrames && wallJumpFrames > 0)
        {
            if (wallJumpFrames < wallJumpFirstFrames)
            {
                ++wallJumpFrames;
                jumpInputUsed = true;
                velocity.y = maxRizeSpeed;
                velocity.x = -wallJumpDir * maxSpeed;
            }
            else if (jumpPressed)
            {
                ++wallJumpFrames;
                jumpInputUsed = true;
                velocity.y = maxRizeSpeed;
            }
            else
                wallJumpFrames = 0;
        }

        if (wallJumpFrames >= maxWallJumpFrames)
            wallJumpFrames = 0;


        if (((dashPressed && dashButtonActive) || dashFrames > 0) && dashFrames < maxDashFrames && dashFrames >= 0)
        {
            if (JumpOrb.readyToJump && JumpOrb.instance != null)
            {
                velocity.x = orbDashCurrentSpeedCoefficient * maxDashSpeed * direction;
                Vector3 forceDirection = transform.position - JumpOrb.instance.transform.position;
                velocity.x += forceDirection.normalized.x * orbDashSpeedCoefficientX * Mathf.Abs(forceDirection.normalized.x);
                velocity.y += forceDirection.normalized.y * orbDashSpeedCoefficientY;

                if (velocity.sqrMagnitude < minOrbBounceMagnitude * minOrbBounceMagnitude)
                    velocity = velocity.normalized * minOrbBounceMagnitude;
                                
                unlimitedJump = true;
                if (maxDashFrames - dashFrames <= orbDashRefreshTiming)
                    dashFromGround = true;
                dashFrames = maxDashFrames;
                JumpOrb.instance.CooldownStart();
            }
            else
            {
                velocity.x = maxDashSpeed * direction;
                velocity.y = 0;
                ++dashFrames;

                dashButtonActive = false;

                if (framesSinceGround <= maxFloatJumpFrames)
                    dashFromGround = true;
            }
        }
        else
        {
            if (dashFrames < 0 || (dashFrames > 0 && dashFrames < dashCooldownFrames + maxDashFrames))
                ++dashFrames;

            if (dashFrames > 0 && dashFrames < maxDashFrames)
                dashFrames = maxDashFrames;

            if (((isGrounded || dashFromGround) || wallRestoresDash && (isWalledL || isWalledR)) && dashFrames > 0)
            {
                dashFrames -= dashCooldownFrames + maxDashFrames;
                dashFromGround = false;
            }
        }

        if (!dashPressed)
            dashButtonActive = true;

        body.velocity = velocity;
    }

    private void isGroundedUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.down, checkBoxOffset, platformLayer);

        Color rayColor;
        if (raycastHit.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y + checkBoxOffset), Vector2.right * playerCollider.bounds.extents.x * 2, rayColor);

        isGrounded = raycastHit.collider != null;
    }

    private void isWalledRUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.right, checkBoxOffset, platformLayer);

        Color rayColor;
        if (raycastHit.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawRay(playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x + checkBoxOffset, playerCollider.bounds.extents.y), Vector2.down * playerCollider.bounds.extents.y * 2, rayColor);

        isWalledR = raycastHit.collider != null;
    }

    private void isWalledLUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.0f, Vector2.left, checkBoxOffset, platformLayer);

        Color rayColor;
        if (raycastHit.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawRay(playerCollider.bounds.center + new Vector3(-playerCollider.bounds.extents.x - checkBoxOffset, playerCollider.bounds.extents.y), Vector2.down * playerCollider.bounds.extents.y * 2, rayColor);

        isWalledL = raycastHit.collider != null;
    }

    public void SetPosition(Vector2 pos)
    {
        if (body)
            body.position = pos;
    }

    public void ResetMovement()
    {
        jumpFrames = 0;
        wallJumpFrames = 0;
        dashFrames = 0;
        isGrounded = false;
        isWalledL = false;
        isWalledR = false;
        framesSinceGround = 0;
        body.velocity = Vector2.zero;
        unlimitedJump = false;
    }
}
