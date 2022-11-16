using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_2 : MonoBehaviour
{
    [Header("Player")]
	[SerializeField] private float moveSpeed = 5.0f;
	[SerializeField] private float sprintSpeed = 6.0f;
	[SerializeField] private float rotationSpeed = 1.0f;
	[SerializeField] private float animationBlendSpeed = 9f;

	[Tooltip("Acceleration and deceleration")]
	[SerializeField] private float speedChangeRate = 10.0f;
	[SerializeField] private float jumpHeight = 1.2f;
	[SerializeField] private float gravity = -15.0f;
	[SerializeField] private float jumpTimeout = 0.1f;
	[SerializeField] private float fallTimeout = 0.15f;
	[SerializeField] private bool isGrounded = true;
	[SerializeField] private float froundedOffset = -0.14f;
	[SerializeField] private float groundedRadius = 0.5f;
	[SerializeField] private LayerMask groundLayers;

	[Header("Cinemachine")]
	[SerializeField] private GameObject cinemachineCameraTarget;
	[SerializeField] private float topClamp = 90.0f;
	[SerializeField] private float bottomClamp = -90.0f;

	private float cinemachineTargetPitch;
	private float speed;
	private float rotationVelocity;
	private float verticalVelocity;
	private float terminalVelocity = 53.0f;
	private float jumpTimeoutDelta;
	private float fallTimeoutDelta;

	
	private InputManager inputManager;
	private CharacterController controller;
	private GameObject mainCamera;
	private const float threshold = 0.01f;

	private Animator animator;
	private Rigidbody playerRigidbody;
	private int velocityXHash;
    private int velocityYHash;
    private int velocityZHash;

    private void Awake()
	{
        inputManager = GetComponent<InputManager>();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		animator = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
		controller = GetComponent<CharacterController>();
	}

	private void Start()
	{
		// optimization
        velocityXHash = Animator.StringToHash("xVelocity");
        velocityYHash = Animator.StringToHash("yVelocity");
        velocityZHash = Animator.StringToHash("zVelocity");
        // jumpHash = Animator.StringToHash("Jump");
        // fallingHash = Animator.StringToHash("Falling");
        // groundedHash = Animator.StringToHash("Grounded");
		
	    jumpTimeoutDelta = jumpTimeout;
		fallTimeoutDelta = fallTimeout;
	}

    private void Update()
	{
		//JumpAndGravity();
		//GroundedCheck();
		Move();
	}

    private void LateUpdate()
	{
		CameraRotation();
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
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

    private void Move()
	{
		float targetSpeed = inputManager.run ? sprintSpeed : moveSpeed;

		if (inputManager.move == Vector2.zero) targetSpeed = 0.0f;

		float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

		float speedOffset = 0.1f;
		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// start * (1 - t) + end * t
			speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
			// round speed
			speed = Mathf.Round(speed * 1000f) / 1000f;
		}
		else
		{
			speed = targetSpeed;
		}

		// direction vector with length of 1
		Vector3 inputDirection = new Vector3(inputManager.move.x, 0.0f, inputManager.move.y).normalized;

		// if there is a move input rotate player when the player is moving
		if (inputManager.move != Vector2.zero)
		{
			inputDirection = transform.right * inputManager.move.x + transform.forward * inputManager.move.y;
		}

		controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

		// var smoothedAnimationVelocityX = inputManager.move.x * targetSpeed * animationBlendSpeed * Time.deltaTime;
		// Debug.Log(smoothedAnimationVelocityX);
		animator.SetFloat(velocityXHash, inputManager.move.x * targetSpeed);
        animator.SetFloat(velocityYHash, inputManager.move.y * targetSpeed);
	}
}
