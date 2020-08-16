using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using DG.Tweening;
using System;

public class MovementController : AdvancedWalkerController
{
	[Header("Core Settings")]
	[SerializeField]
	private MovementInput _movementInput;
	[SerializeField]
	private RaycastSide? rSide;

	[Header("Dash Settings")]
	[SerializeField]
	private float _dashForce;
	[SerializeField]
	private float _dashDuration;
	[SerializeField]
	private float _dashCooldown;

	[Header("Booleans")]
	[SerializeField]
	private bool canJump;
	[SerializeField]
	private bool _canDash;
	[SerializeField]
	private bool _isOnWall;
	[SerializeField]
	private bool _dashRoutineIsEnd = true;

	protected override void Update()
	{
		base.Update();

		CheckDash();
	}

	protected override void FixedUpdate()
	{
		//Check if mover is grounded;
			
		mover.CheckForGround();

		//Determine controller state;
		CurrentControllerState = DetermineControllerState();

		//Apply friction and gravity to 'momentum';
		HandleMomentum();

		//Check if the player has initiated a jump;
		HandleJumping();

		//Calculate movement velocity;
		Vector3 _velocity = CalculateMovementVelocity();
		
		if(_canDash)
			_velocity += HandleDash();

			//If local momentum is used, transform momentum into world space first;
			Vector3 _worldMomentum = momentum;
			if (useLocalMomentum)
				_worldMomentum = tr.localToWorldMatrix * momentum;

			//Add current momentum to velocity;
			_velocity += _worldMomentum;

			//If player is grounded or sliding on a slope, extend mover's sensor range;
			//This enables the player to walk up/down stairs and slopes without losing ground contact;
			mover.SetExtendSensorRange(IsGrounded());

			//Set mover velocity;		
			mover.SetVelocity(_velocity);

			//Store velocity for next frame;
			savedVelocity = _velocity;
			savedMovementVelocity = _velocity - _worldMomentum;

			//Reset jump key booleans;
			jumpKeyWasLetGo = false;
			jumpKeyWasPressed = false;

			//Reset ceiling detector, if one was attached to this gameobject;
			if (ceilingDetector != null)
				ceilingDetector.ResetFlags();
	}

    private void CheckDash()
    {
		if(!_dashRoutineIsEnd) return;

        _canDash = _movementInput.IsDashButtonPressed();

		Debug.Log("Can dash " + _canDash);

		if(_canDash)
			StartCoroutine(DashRoutine());
    }

	private Vector3 HandleDash()
	{
		if(_canDash)
		{
			Debug.Log($"Vector dash {GetDirection()  * _dashForce}");
			return GetDirection()  * _dashForce;
		}

		return Vector3.zero;
	}

	IEnumerator DashRoutine()
	{
		_dashRoutineIsEnd = false;

		yield return new WaitForSeconds(_dashDuration);

		_canDash = false;

		StartCoroutine(DashCooldownRoutine());

		StopCoroutine(DashRoutine());
	}

	IEnumerator DashCooldownRoutine()
	{
		yield return new WaitForSeconds(_dashCooldown);

		_dashRoutineIsEnd = true;

		StopCoroutine(DashCooldownRoutine());
	}

	private Vector3 GetDirection()
	{
		return cameraTransform.forward;
	}

    protected override void Setup()
	{
		OnStateChanged += StateChanged;
		_dashRoutineIsEnd = true;
		base.Setup();
	}

	public void WallHit(Transform wall, RaycastSide side)
	{
		float rotZ = side == RaycastSide.Left ? -12.5f : 12.5f;

		Vector3 rotVector = new Vector3(tr.eulerAngles.x, tr.eulerAngles.y, rotZ);

		canJump = true;

		if (wall == null)
		{
			_isOnWall = false;
			rSide = null;
			rotVector.z = 0;
		}
		else
		{
			_isOnWall = true;
			rSide = side;
			CurrentControllerState = ControllerState.Grounded;
		}

		transform.DORotate(rotVector,0.5f);
	}

	protected override void HandleJumping()
	{
		if (CurrentControllerState == ControllerState.Grounded || canJump)
		{
			if (jumpKeyIsPressed == true || jumpKeyWasPressed)
			{
				//Call events;
				OnGroundContactLost();
				OnJumpStart();

				CurrentControllerState = ControllerState.Jumping;
			}
		}
	}

	void StateChanged(ControllerState controllerState)
	{
		switch (controllerState)
		{
			case ControllerState.Grounded:
				canJump = true;
				break;
			case ControllerState.Sliding:
				break;
			case ControllerState.Falling:
				break;
			case ControllerState.Rising:
				break;
			case ControllerState.Jumping:
				canJump = false;
				break;
			default:
				break;
		}
	}

	protected override void OnJumpStart()
	{
		//If local momentum is used, transform momentum into world coordinates first;
		if (useLocalMomentum)
			momentum = tr.localToWorldMatrix * momentum;

		//Add jump force to momentum;
		momentum += GetDirectionFromSide() * jumpSpeed;

		//Set jump start time;
		currentJumpStartTime = Time.time;

		//Call event;
		if (OnJump != null)
			OnJump(momentum);

		if (useLocalMomentum)
			momentum = tr.worldToLocalMatrix * momentum;
	}

	private Vector3 GetDirectionFromSide()
	{
		if (!rSide.HasValue)
			return tr.up;

		if (rSide.Value == RaycastSide.Left)
			return tr.right;
		else
			return -tr.right;
	}
}
