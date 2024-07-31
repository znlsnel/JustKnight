using Cainos.PixelArtPlatformer_VillageProps;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.Playables;
using static PlayerAnimCtrl;

public enum EActiveState : int
{
	Awaiting, 
	Attack,
	Jump,
	Roll,
	Shield,
	Hit,
}



public class PlayerActionController : MonoBehaviour
{
	PlayerMovementController _movementController;
	PlayerAnimCtrl _animCtrl;
	PlayerController _playerController;
	InputManager _inputManager;
	PlayerCollisionManager _playerCollision;

	public EActiveState _activeState = EActiveState.Awaiting;

	[NonSerialized] public int _attackCombo = 1;

	bool _isAttackable = true;
	bool _isRollable = true;
	bool _isShieldable = true; 
	 
	public Action _onPortalEntered;

	private void Start()
	{
		_movementController = GetComponent<PlayerMovementController>();
		_playerController = GetComponent<PlayerController>();
		_playerCollision = GetComponent<PlayerCollisionManager>();
		_animCtrl = GetComponent<PlayerAnimCtrl>();
		_inputManager = InputManager.instance;

		_inputManager.BindInputAction("Attack", () => InputAttack(true), () => InputAttack(false));
		_inputManager.BindInputAction("Shield", () => InputShield(true), ()=>InputShield(false)); 
		_inputManager.BindInputAction("Jump", InputJump);
		_inputManager.BindInputAction("Roll", InputRoll);
		_inputManager.BindInputAction("Up", InputPortal); 
	}

	private void Update()
	{

	}

	bool CHECK(ref bool usable, EActiveState state)
	{
		EPlayerState playerState = _playerController._playerState;
		if (usable == false || _activeState != EActiveState.Awaiting ||  
			playerState == EPlayerState.Fall || playerState == EPlayerState.Death)
			return false;

		usable = false;
		_activeState = state;

		return true;
	}
	 
	IEnumerator RegisterCooldown(float time, Action finishAction)
	{  
		yield return new WaitForEndOfFrame();
		float animLength = _animCtrl.GetCurAnimLength();

		yield return new WaitForSeconds(animLength);
		_activeState = EActiveState.Awaiting;
		_animCtrl.PlayAnimation();

		yield return new WaitForSeconds(time); 
		finishAction.Invoke(); 
	}
	  
	void InputPortal()
	{
		_onPortalEntered?.Invoke();
		_onPortalEntered = null; 
	}

	Coroutine attackCT; 
	void InputAttack(bool press)
	{
		if (!press)
		{
			if (attackCT != null)
			{
				StopCoroutine(attackCT);
				attackCT = null; 
			}
			return;
		}
		 
	//	Debug.Log("Attack");
		if (!CHECK(ref _isAttackable, EActiveState.Attack))
			return;

		attackCT = StartCoroutine(OnAttack());
	}

	IEnumerator OnAttack()
	{
		_animCtrl.PlayAnimation($"Attack{_attackCombo + 1}");
		_attackCombo = (_attackCombo + 1) % 3;
		_playerController._playerState = EPlayerState.Idle; 

		yield return StartCoroutine(RegisterCooldown(_playerController._attackDelay, () => { _isAttackable = true; }));

		if (!InputManager.instance.GetInputAction("Attack").IsPressed()) 
			yield break;

		InputAttack(true);
	}


	void InputJump()
	{
		bool isGround = Mathf.Abs(_movementController._rigidbody.velocity.y) <= 0.1f;
		if (!CHECK(ref isGround, EActiveState.Jump)) 
			return;

		OnJump(); 
	}

	public float jumpSize = 4.0f;  
	public void OnJump(bool isSideJump = false)
	{
		_activeState = EActiveState.Jump;
		_playerController._playerState = EPlayerState.Idle;
		Rigidbody2D rb = _movementController._rigidbody;
		 
		_playerController.r_Jump.Invoke();

		float moveDir = 0.0f; 
		if (isSideJump)
		{
			moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>() * -jumpSize;
		}
		rb.AddForce(new Vector2(moveDir, 8.0f), ForceMode2D.Impulse);
		_animCtrl.PlayAnimation("Jump");
	}

	public void OnFallStart()
	{
		_animCtrl.anim.speed = 1.0f;
	}

	public void OnLand()
	{
		if (_activeState == EActiveState.Jump)
			_activeState = EActiveState.Awaiting; 
	}
		
	void InputRoll()
	{
		if (!CHECK(ref _isRollable, EActiveState.Roll))
			return;

		_playerController._playerState = EPlayerState.Idle;
		_movementController._rigidbody.AddForce(new Vector2(gameObject.transform.localScale.x * 10.0f, 0.0f), ForceMode2D.Impulse);

		_animCtrl.PlayAnimation("Roll");
		StartCoroutine(RegisterCooldown(_playerController._rollDelay, () => { _isRollable = true; })); 
	}

	void InputShield(bool press)
	{
		if (!CHECK(ref _isShieldable, EActiveState.Shield))
			return; 
		 
		  
		_animCtrl.PlayAnimation("Shield");
		StartCoroutine(RegisterCooldown(_playerController._shieldDelay, () => { _isShieldable = true; }));
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
				continue;
			
			mc.OnHit(gameObject);
			if (mc._hp == 0) 
				ItemManager.instance.GetItemObj(mc.gameObject.transform.position);
		}
	}
	
	void AE_EndAttack()
	{ 

	}

	void AE_EndRoll()
	{
	} 

	void AE_EndJump() 
	{
		_animCtrl.anim.speed = 0.0f;
	}

	void AE_EndShield()
	{

		//Vector3 temp = _movementController._rigidbody.velocity;
		//temp.y = 0.0f; 
		//_movementController._rigidbody.velocity = temp;
	}

	float _lastHitTime = 0.0f;
	public float _damageCooldown = 0.5f;
	public void OnHit(GameObject monster)
	{
		if (Time.time - _lastHitTime < _damageCooldown || _activeState == EActiveState.Roll || _playerController. hp == 0)
			return;
		 
		_lastHitTime = Time.time;
		if (_activeState == EActiveState.Shield)
		{
			Monster mst = monster.GetComponent<Monster>();
			if (mst != null)
			{ 
				mst._onAttackBlocked = () => { mst.OnHit(gameObject); };

			}
			return;
		}
		 
		_playerController.hp--;

		CameraManager cm = Camera.main.GetComponent<CameraManager>();
		cm?.ShakeCamera(monster.transform.position.x < gameObject.transform.position.x);

		_activeState = EActiveState.Hit;
		_animCtrl.PlayAnimation("Hit");
		
	}

	void AE_EndHit()
	{
		_animCtrl.PlayAnimation(); 
		_activeState = EActiveState.Awaiting;
		  
		if (_playerController.hp == 0)
			_playerController._playerState = EPlayerState.Death;
	}

}
