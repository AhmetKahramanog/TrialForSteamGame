using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BasePlayerManager
{
    private float currentSpeed;
    [SerializeField] private float walkSpeed;
    Vector3 input;
    [SerializeField] private Transform cam;
    private Vector3 targetDirection;
    [SerializeField] private float rotationSpeed;
    private Rigidbody playerRB;
    [SerializeField] private float jumpForce;
    bool isGrounded = true;
    private Animator animator;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private bool IsCrouching = false;
    private bool isFalling = false;
    [SerializeField] private LayerMask groundLayer;
    private bool isDown = false;
    private PlayerAttack playerAttack;
    private Vector3 movementDirection;


    void Start()
    {
        IsStopAllFunc = false;
        playerRB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
        playerAttack = GetComponent<PlayerAttack>();
    }
    void Update()
    {
        if (PlayerHealthBar.isDie) { return; }

        if (playerAttack.IsAttacking) { return; }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            JumpAction(jumpForce);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
            animator.SetBool("IsRun", false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();
        }
        animator.SetBool("IsCrouch", IsCrouching);
        animator.SetFloat("CrouchWalk", MoveSpeed());
        isFalling = Physics.CheckSphere(transform.position, 5.0f,groundLayer);
        Fall();
        isDown = Physics.CheckSphere(transform.position, 2.0f, groundLayer);
        

    }

    private void FixedUpdate()
    {
        if (PlayerHealthBar.isDie) { return; }

        if (playerAttack.IsAttacking) { return; }

        RotationHandle();
        MovementHandle();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }
    }

    private void MovementHandle()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");
        movementDirection = cam.transform.forward * input.z;
        movementDirection += cam.transform.right * input.x;
        animator.SetBool("IsMove", input != Vector3.zero ? true : false);
        movementDirection.y = 0f;
        transform.Translate(movementDirection * currentSpeed * Time.fixedDeltaTime,Space.World);
        MoveSpeed();
    }


    private void RotationHandle()
    {
        targetDirection = Vector3.zero;

        targetDirection = cam.transform.forward * input.z;
        targetDirection += cam.transform.right * input.x;
        targetDirection.Normalize();
        targetDirection.y = 0f;
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
        var target = Quaternion.LookRotation(targetDirection);
        var targetSlerp = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = targetSlerp;
    }

    private void JumpAction(float force)
    {
        playerRB.AddForce(Vector3.up * force,ForceMode.Impulse);
        animator.SetTrigger("Jumping");
    }


    private void Run()
    {
        currentSpeed = runSpeed;
        animator.SetBool("IsRun", true);
        IsCrouching = false;
    }

   private void Fall()
    {
        float verticalVeloctiy = playerRB.velocity.y;
        if (!isGrounded && verticalVeloctiy < 0f && !isFalling)
        {
            animator.SetTrigger("Falling");
        }
        if (!isGrounded && isDown)
        {
            animator.SetTrigger("Standing");
        }
    }

    private void Crouch()
    {
        IsCrouching = !IsCrouching;
        if (IsCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
            playerAttack.CanAttack = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
            playerAttack.CanAttack = false;
        }
    }

    public float MoveSpeed()
    {
        return movementDirection.magnitude * currentSpeed;
    }
}
