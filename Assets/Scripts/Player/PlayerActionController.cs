using Cainos.PixelArtPlatformer_VillageProps;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.Events;
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
	PlayerStatus _status;

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
		_status = PlayerStatus.instance;

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
	 
	IEnumerator RegisterCooldown(float time, Action finishAction = null, int cnt = 2)
	{
		
		while (cnt-- > 0) // 1.0f는 잘못된 길이의 기본값이라고 가정
			yield return null; // 다음 프레임까지 대기

		float animLength = _animCtrl.GetCurAnimLength();
		//Debug.Log(animLength);
		yield return new WaitForSeconds(animLength);
		_activeState = EActiveState.Awaiting;
		_animCtrl.PlayAnimation();

		yield return new WaitForSeconds(time);  
		finishAction?.Invoke(); 
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
		

		attackCT = StartCoroutine(OnAttack());
	}

	IEnumerator OnAttack()
	{
		if (CHECK(ref _isAttackable, EActiveState.Attack))
		{
			_attackCombo = (_attackCombo + 1) % 3;
			_playerController._playerState = EPlayerState.Idle;
			_animCtrl.PlayAnimation($"Attack{_attackCombo + 1}");
			_animCtrl.anim.speed = (float)PlayerStatus.instance.GetValue(EPlayerStatus.AttackSpeed) * 0.01f;
			_playerController.qr_attack?.Invoke();


		}
		 

		float coolTime = _playerController._attackDelay * (float)PlayerStatus.instance.GetValue(EPlayerStatus.AttackSpeed) * 0.01f;
		yield return StartCoroutine(RegisterCooldown(coolTime, () => {
			_isAttackable = true;
			_animCtrl.anim.speed = 1.0f;
		}));
		 
		if (InputManager.instance.GetInputAction("Attack").IsPressed())
		{
			InputAttack(true); 
		} 
	}


	void InputJump()
	{
		bool isGround = Mathf.Abs(_movementController._rigidbody.velocity.y) <= 0.1f;
		if (!CHECK(ref isGround, EActiveState.Jump)) 
			return;

		OnJump(); 
	}

	public void OnJump(bool isSideJump = false)
	{
		_activeState = EActiveState.Jump;
		_playerController._playerState = EPlayerState.Idle;
		Rigidbody2D rb = _movementController._rigidbody;
		 
		_playerController.qr_Jump.Invoke();

		float moveDir = isSideJump ? InputManager.instance.GetInputAction("Move").ReadValue<float>() * - 4.0f : 0.0f;

		float jumpSize = _playerController._jumpPower * (float)PlayerStatus.instance.GetValue(EPlayerStatus.jumpPower) * 0.01f;
		rb.AddForce(new Vector2(moveDir, jumpSize), ForceMode2D.Impulse);
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
		_movementController._rigidbody.AddForce(new Vector2(gameObject.transform.localScale.x * _playerController._rollPower, 0.0f), ForceMode2D.Impulse);

		_animCtrl.PlayAnimation("Roll");
		StartCoroutine(RegisterCooldown(_playerController._rollDelay, () => { _isRollable = true; }));
		_playerController.qr_roll.Invoke();
	}

	void InputShield(bool press)
	{
		if (!CHECK(ref _isShieldable, EActiveState.Shield))
			return; 
		 
		  
		_animCtrl.PlayAnimation("Shield");
		_playerController.qr_shield?.Invoke(); 
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
			if (mc != null)
			{
				mc.OnHit(gameObject, _status.GetValue(EPlayerStatus.Damage));
				bool dubbleAttack = UnityEngine.Random.Range(0, 100) < PlayerStatus.instance.GetValue(EPlayerStatus.DubbleAttackRate);
				if (dubbleAttack)
					Utils.instance.SetTimer(() => { mc.OnHit(gameObject, _status.GetValue(EPlayerStatus.Damage));}, 0.1f );
			}
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

	public void OnHit(GameObject monster, int damage = 1)
	{
		
		if (Time.time - _lastHitTime < _damageCooldown || 
			_activeState == EActiveState.Roll || 
			_playerController._playerState == EPlayerState.Death)
			return;
		 
		_lastHitTime = Time.time;
		if (_activeState == EActiveState.Shield)
		{
			Monster mm= monster.GetComponent<Monster>();
			if (mm != null)
			{
				mm._onAttackBlocked = () => { mm.OnHit(gameObject, _status.GetValue(EPlayerStatus.ShieldDamage), true); };
				
				_playerController.qr_successShield?.Invoke(); 
			} 
			return; 
		}

		bool avoid = UnityEngine.Random.Range(0, 100) < PlayerStatus.instance.GetValue(EPlayerStatus.AvoidanceRate);

		if (!avoid) 
		{
			_playerController.hp -= Mathf.Max(0, damage - _status.GetValue(EPlayerStatus.Armor));

			CameraManager cm = Camera.main.GetComponent<CameraManager>();
			cm?.ShakeCamera(monster.transform.position.x < gameObject.transform.position.x);

			_activeState = EActiveState.Hit;
			_playerController._playerState = EPlayerState.Idle;
			_animCtrl.PlayAnimation("Hit");
;
			StartCoroutine(RegisterCooldown(0.0f, () => { 
				if (_playerController.hp == 0)
				{
					_playerController._playerState = EPlayerState.Death;
					UIHandler.instance._dieUI.GetComponent<DieUI>().Open();
				}

				_animCtrl.PlayAnimation();
				_activeState = EActiveState.Awaiting;
			}));
		}
	}

	void AE_EndHit()
	{

		
	}

}
