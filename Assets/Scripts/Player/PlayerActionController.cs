using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimCtrl;

public enum EActiveState : int
{
	None,
	Attack,
	Jump,
	Roll,
	Shield,
}

public class PlayerActionController : MonoBehaviour
{
	PlayerMovementController _MovementController;
	PlayerController _PlayerController;
	InputManager _inputManager;
	PlayerCollisionManager _playerCollision;

	EActiveState _activeState = EActiveState.None;

	float _lastAttackTime = 100.0f;
	float _lastJumpTime = 100.0f;
	float _lastRollTime = 100.0f;
	float _lastShieldTime = 100.0f;

	private void Start()
	{
		_MovementController = GetComponent<PlayerMovementController>();
		_PlayerController = GetComponent<PlayerController>();
		_playerCollision = GetComponent<PlayerCollisionManager>();
		_inputManager = InputManager.instance;

		_inputManager.BindInputAction("Attack", () => InputAttack(true), () => InputAttack(false));
		_inputManager.BindInputAction("Shield", () => InputShield(true), ()=>InputShield(false)); 
		_inputManager.BindInputAction("Jump", InputJump);
		_inputManager.BindInputAction("Roll", InputRoll);
	}
	private void Update()
	{
		if (_activeState != EActiveState.Attack)
			_lastAttackTime += Time.deltaTime;
		_lastJumpTime += Time.deltaTime; 
		_lastRollTime += Time.deltaTime;
		_lastShieldTime += Time.deltaTime; 
	}


	void InputAttack(bool press)
	{
		if (CHECK(new [] { EPlayerState.Fall, EPlayerState.Death}, 
				  new [] { EActiveState.Roll, EActiveState.Shield })) 
			return;

		if (_lastAttackTime > _PlayerController._attackDelay)
			return;
		 
		_lastAttackTime = 0.0f;
		_activeState = EActiveState.Attack;
	}

	void InputJump()
	{
		_activeState = EActiveState.Jump;

	}

	void InputRoll()
	{
		_activeState = EActiveState.Roll;

	}

	void InputShield(bool press)
	{
		_activeState = EActiveState.Shield; 

	}

	bool CHECK(EPlayerState[] playerStates, EActiveState[] activeStates)
	{
		EPlayerState playerState = _MovementController._playerState;

		foreach (EPlayerState state in playerStates)
		{
			if (playerState == state)
				return false;
		}

		foreach (EActiveState state in activeStates)
		{
			if (_activeState == state)
				return false;
		}

		return true;
	}

	void AE_EndAttack()
	{
		_activeState = EActiveState.None;
	}


	void AE_OnAttack()
	{
		Collider2D[] result = new Collider2D[100];
		ContactFilter2D contactFilter = new ContactFilter2D();
		Physics2D.OverlapCollider(_playerCollision._sensorAttack, contactFilter, result);

		foreach (var c in result)
		{
			if (c == null)
				break;

			Monster mc = c.gameObject.GetComponent<Monster>();
			if (mc == null || mc._hp == 0)
				return;
			
			mc.OnHit(gameObject);
			if (mc._hp == 0) 
				ItemManager.instance.GetItemObj(mc.gameObject.transform.position);
		}
	}
}
