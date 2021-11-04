using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StarterAssets;


/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class CameraGhost : MonoBehaviour
{
	[Header("Ghost Cam")]
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 4.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	public float SprintSpeed = 6.0f;
	[Tooltip("Crouch speed of the character in m/s")]
	public float CrouchSpeed = 2.0f;
	[Tooltip("Rotation speed of the character")]
	public float RotationSpeed = 1.0f;
	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 90.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -90.0f;

	// cinemachine
	private float _cinemachineTargetPitch;

	// player
	private float _speed;
	private float _rotationVelocity;

	private CharacterController _controller;
	private StarterAssetsInputs _input;
	private GameObject _mainCamera;

	private const float _threshold = 0.01f;

	private void Awake()
	{
		// get a reference to our main camera
		if (_mainCamera == null)
		{
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}

	private void Start()
	{
		_controller = GetComponent<CharacterController>();
		_controller.detectCollisions = false;
		_input = GetComponent<StarterAssetsInputs>();
	}

	private void Update()
	{
		Move();
	}

	private void LateUpdate()
	{
		CameraRotation();
	}

	private void CameraRotation()
	{
		// if there is an input
		if (_input.look.sqrMagnitude >= _threshold)
		{
			_cinemachineTargetPitch += _input.look.y * RotationSpeed * Time.deltaTime;
			_rotationVelocity = _input.look.x * RotationSpeed * Time.deltaTime;

			// clamp our pitch rotation
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Update Cinemachine camera target pitch
			CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

			// rotate the player left and right
			transform.Rotate(Vector3.up * _rotationVelocity);
		}
	}

	private void Move()
	{
		// set target speed based on move speed, sprint speed and if sprint is pressed
		float speed = _input.sprint ? SprintSpeed : MoveSpeed;
		

		float vertical;
		if (_input.jump) vertical = 2.0f;
		else if (_input.crouch) vertical = -2.0f;
		else vertical = 0.0f;

		if (_input.move == Vector2.zero && vertical == 0.0f) speed = 0.0f;

		Vector3 vDirection = new Vector3(0.0f, vertical, 0.0f).normalized;

		float currentHorizontalSpeed = new Vector3(_controller.velocity.z, 0.0f, _controller.velocity.x).magnitude;

		float speedOffset = 0.1f;
		float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < speed - speedOffset || currentHorizontalSpeed > speed + speedOffset)
		{
			_speed = Mathf.Lerp(currentHorizontalSpeed, speed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = speed;
		}


		Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
		if (_input.move != Vector2.zero)
		{
			inputDirection = transform.right * _input.move.x + CinemachineCameraTarget.transform.forward * _input.move.y;
		}

		// move the player
		_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + vDirection * (_speed * Time.deltaTime));
	}



	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class CameraGhost : MonoBehaviour
{
    public Transform cam;

    [Header("Movement Settings")]
    public float MoveSpeed = 4.0f;
    public float FastSpeed = 6.0f;

    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;

    private float cmTargetYaw;
    private float cmTargetPitch;

    private float RotationSmoothTime = 0.12f;
    private float targetRotation = 0.0f;
    private float rotationVelocity;

    private const float threshold = 0.01f;

    private StarterAssetsInputs input;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        input = GetComponent<StarterAssetsInputs>();
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Movement();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        if (input.look.sqrMagnitude >= threshold && !LockCameraPosition)
        {
            cmTargetYaw += input.look.x * Time.deltaTime;
            cmTargetPitch += input.look.y * Time.deltaTime;
        }

        cmTargetYaw = ClampAngle(cmTargetYaw, float.MinValue, float.MaxValue);
        cmTargetPitch = ClampAngle(cmTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cmTargetPitch + CameraAngleOverride, cmTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void Movement()
    {
        float speed = input.sprint ? FastSpeed : MoveSpeed;
        if (input.move == Vector2.zero) speed = 0.0f;

        float vertical;
        if (input.jump) vertical = 1.0f;
        else if (input.crouch) vertical = -1.0f;
        else vertical = 0.0f;

        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;
        Vector3 vDirection = new Vector3(0.0f, vertical, 0.0f).normalized;

        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        cc.Move(targetDirection.normalized * (speed * Time.deltaTime) + vDirection * Time.deltaTime);
    }
}*/
