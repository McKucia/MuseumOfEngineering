using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animationBlendSpeed = 9f;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float cameraUpperLimit = -40f;
    [SerializeField] private float cameraBottomLimit = 70f;
    [SerializeField] private float mouseSensivity = 40f;

    private Rigidbody playerRigidbody;
    private InputManager inputManager;
    private Animator animator;
    private int velocityX;
    private int velocityY;

    private const float runSpeed = 5f;
    private const float walkSpeed = 1.5f;
    private float rotationX;
    private Vector2 currentVelocity;
    
    private void Start() {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();

        // optimization
        velocityX = Animator.StringToHash("xVelocity");
        velocityY = Animator.StringToHash("yVelocity");
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraMovements();
    }
    
    private void Move()
    {
        float targetSpeed = inputManager.run ? runSpeed : walkSpeed;
        if(inputManager.move == Vector2.zero) targetSpeed = 0;

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.move.x * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);
        currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.move.y * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);
        
        var smoothedVelocityX = currentVelocity.x - playerRigidbody.velocity.x;
        var smoothedVelocityZ = currentVelocity.y - playerRigidbody.velocity.z;

        playerRigidbody.AddForce(transform.TransformVector(new Vector3(smoothedVelocityX, 0 , smoothedVelocityZ)), ForceMode.VelocityChange);

        animator.SetFloat(velocityX, currentVelocity.x);
        animator.SetFloat(velocityY, currentVelocity.y);
    }

    private void CameraMovements()
    {
        var mouseX = inputManager.look.x;
        var mouseY = inputManager.look.y;

        playerCamera.position = cameraRoot.position;

        rotationX -= mouseY * mouseSensivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, cameraUpperLimit, cameraBottomLimit);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up, mouseX * mouseSensivity * Time.deltaTime);
    }
}
