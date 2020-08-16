using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using DG.Tweening;

public class MovementController : AdvancedWalkerController
{
	[SerializeField]
	private RaycastSide? rSide;

	[SerializeField]
	private bool canJump;

	protected override void Setup()
	{
		OnStateChanged += StateChanged;

		base.Setup();
	}

	public void WallHit(Transform wall, RaycastSide side)
	{
		float rotZ = side == RaycastSide.Left ? -12.5f : 12.5f;

		Vector3 rotVector = new Vector3(tr.eulerAngles.x, tr.eulerAngles.y, rotZ);

		canJump = true;

		if (wall == null)
		{
			rSide = null;
			rotVector.z = 0;
		}
		else
		{
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
