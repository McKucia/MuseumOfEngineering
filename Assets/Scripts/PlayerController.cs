using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Cinemachine")]
	[SerializeField] private GameObject cinemachineCameraTarget;
	[SerializeField] private float topClamp = 90.0f;
	[SerializeField] private float bottomClamp = -90.0f;
    private const float threshold = 0.01f;
    private float cinemachineTargetPitch;
	private float rotationVelocity;
    [SerializeField] private float rotationSpeed = 1.0f;

    [SerializeField] private float animationBlendSpeed = 9f;
    [SerializeField] private float cameraPositionChangeSpeed = 9f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float jumpFactor;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask; 
    [SerializeField] private float airResistance = 0.8f;
    [SerializeField] private float gravity;

    public bool canMove { get; set;} = true;

    private Rigidbody playerRigidbody;
    private InputManager inputManager;
    private Animator animator;
    private int velocityXHash;
    private int velocityYHash;
    private int velocityZHash;
    private int jumpHash;
    private int fallingHash;
    private int groundedHash;
    private bool isGrounded;

    private const float runSpeed = 5f;
    private const float walkSpeed = 1.5f;
    private float rotationX;
    private Vector2 currentVelocity;
    
    private void Start() {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();

        // optimization
        velocityXHash = Animator.StringToHash("xVelocity");
        velocityYHash = Animator.StringToHash("yVelocity");
        velocityZHash = Animator.StringToHash("zVelocity");
        jumpHash = Animator.StringToHash("Jump");
        fallingHash = Animator.StringToHash("Falling");
        groundedHash = Animator.StringToHash("Grounded");
    }
    private void FixedUpdate()
    {
        CheckGround();
        if(canMove)
        {
            Move();
            Jump();
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

    private void LateUpdate()
    {
        CameraRotation();
    }
    
    private void Move()
    {
        float targetSpeed = inputManager.run ? runSpeed : walkSpeed;
        
        if(inputManager.move == Vector2.zero) targetSpeed = 0;

        if(isGrounded)
        {
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.move.x * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);
            currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.move.y * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);
            
            var smoothedVelocityX = currentVelocity.x - playerRigidbody.velocity.x;
            var smoothedVelocityZ = currentVelocity.y - playerRigidbody.velocity.z;

            playerRigidbody.AddForce(transform.TransformVector(new Vector3(smoothedVelocityX, 0 , smoothedVelocityZ)), ForceMode.VelocityChange);
        }
        else
        {
            playerRigidbody.AddForce(transform.TransformVector(new Vector3(currentVelocity.x * airResistance, 0 ,currentVelocity.y * airResistance)), ForceMode.VelocityChange);
            playerRigidbody.AddForce(-Vector3.up * gravity);
        }

        var cameraLocalPosition = cinemachineCameraTarget.transform.localPosition;
        float targetCameraOffset = 0.15f;

        if(inputManager.move != Vector2.zero) {
            targetCameraOffset = inputManager.run ? 0.6f : 0.35f;
        }

        var targetCameraPosZ = Mathf.Lerp(cameraLocalPosition.z, targetCameraOffset, cameraPositionChangeSpeed * Time.fixedDeltaTime);
        cinemachineCameraTarget.transform.localPosition = new Vector3(cameraLocalPosition.x, cameraLocalPosition.y, targetCameraPosZ);

        animator.SetFloat(velocityXHash, currentVelocity.x);
        animator.SetFloat(velocityYHash, currentVelocity.y);
    }

    private void CameraRotation()
	{
		if (inputManager.look.sqrMagnitude >= threshold)
		{	
			cinemachineTargetPitch -= inputManager.look.y * rotationSpeed;
			rotationVelocity = inputManager.look.x * rotationSpeed;
			cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);
			cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);
			// rotate the player left and right
			transform.Rotate(Vector3.up * rotationVelocity);
		}
	}

    private void Jump()
    {
        if(!inputManager.jump) return;
        if(!isGrounded) return;
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;

        animator.SetTrigger(jumpHash);
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerRigidbody.worldCenterOfMass, Vector3.down, out hit, groundDistance + 0.1f, groundMask))
        {
            isGrounded = true;
            animator.SetBool(fallingHash, !isGrounded);
            animator.SetBool(groundedHash, isGrounded);
            return;
        }
        isGrounded = false;
        animator.SetFloat(velocityZHash, playerRigidbody.velocity.y);
        animator.SetBool(fallingHash, !isGrounded);
        animator.SetBool(groundedHash, isGrounded);

        return;
    }

    public void JumpAddForce()
    {
        playerRigidbody.AddForce(-playerRigidbody.velocity.y * Vector3.up, ForceMode.VelocityChange);
        playerRigidbody.AddForce(Vector3.up * jumpFactor, ForceMode.Impulse);
        animator.ResetTrigger(jumpHash);
    }
}
