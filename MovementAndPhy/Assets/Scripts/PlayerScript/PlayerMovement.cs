using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;

    public float jumpForce;
    public float jumpCoolDown;
    public float jumpAirMultiplyer;

    bool isReadyToJump;

    public Transform orientation;

    float horizontalInput, verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    //private Animator anim;

    [Header("Check player on the groun")]
    public float playerHeight;
    public LayerMask whatIsGrounded;
    bool grounded;

    public float groundDrag;

    public KeyCode jumpkey = KeyCode.Space;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        isReadyToJump = true;
    }

    private void Update()
    {
        // Grounded check ?
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGrounded);
        MyInput();
        SpeedControl();

        // handle drag if grounded
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (isReadyToJump && Input.GetKeyDown(jumpkey) && grounded)
        {
            isReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    private void MovePlayer()
    {
        Vector3 lookRotation = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Calculate movement direction
      //  moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(lookRotation * moveSpeed * 10, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(lookRotation * jumpAirMultiplyer * 10, ForceMode.Force);

     


        if (lookRotation != Vector3.zero)
        {
            // Do the rotation here
            Quaternion targetRotation = Quaternion.LookRotation(lookRotation);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.fixedDeltaTime);
            rb.MoveRotation(targetRotation);
        }

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }
}
