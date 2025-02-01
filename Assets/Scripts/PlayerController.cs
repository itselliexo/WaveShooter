using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed;
    [SerializeField] float defaultSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float backwardsSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float smoothingTime;
    [SerializeField] GameObject cameraObj;
    private float currentHorizontalVelocity;
    //float updateTime = 1f;
    //float timeSinceUpdate = 0f;

    [SerializeField] CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector3 currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        speed = defaultSpeed;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        /* timeSinceUpdate += Time.deltaTime;
         if (timeSinceUpdate > updateTime)
         {
             Debug.Log(speed);
             timeSinceUpdate = 0f;
         }*/
    }

    private void Movement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");
        float sprint = Input.GetAxis("Sprint");

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            currentHorizontalVelocity = Mathf.Lerp(currentHorizontalVelocity, 0f, Time.deltaTime / smoothingTime);
        }
        else
        {
            currentHorizontalVelocity = moveX;
        }

        Vector3 targetDirection = transform.right * currentHorizontalVelocity + transform.forward * moveZ;

        Vector3 smoothedDirection = Vector3.SmoothDamp(currentDirection, targetDirection, ref currentDirection, smoothingTime);

        smoothedDirection.y = 0f;

        characterController.Move(smoothedDirection * speed * Time.deltaTime);

        if (jump > 0 && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (isGrounded)
        {
            if (moveZ > 0 && sprint > 0)
            {
                speed = sprint > 0 ? sprintSpeed : defaultSpeed;
            }
            else if (moveZ < 0)
            { 
                speed = moveZ < 0 ? backwardsSpeed : defaultSpeed;
            }
            else
            {
                speed = defaultSpeed;
            }
        }
    }
}
