using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static PlayerAnimCtrl;

public class PlayerController : MonoBehaviour
{
	PlayerAnimCtrl _animCtrl;
	PlayerInfoUIManager _infoUIManager;
	PlayerCollisionManager _collMan; 
	InputManager _inputManager;

	[SerializeField] float _attackCoolTime = 0.2f;
	[SerializeField] float _jumpCoolTime = 0.2f;
	[SerializeField] float _rollCoolTime = 1.0f;
	[SerializeField] float _blockCoolTime = 1.0f;

	[SerializeField] float _playerSpeed = 5.0f;

	[SerializeField] GameObject _SlideDust;

	public Action _onPortalEntered;

	Rigidbody2D _rigid;

	Vector2 _moveDir = Vector2.zero;

	float _xMoveDir = 0.0f;




	bool _isAttack = false;
	bool _isJump = false;
	bool _isRoll = false;
	bool _isShield = false; 

	bool _isAttackable = true;

	int hp = 3;

	private float _lastAttackTime = 100.0f;
	private float _lastJumpTime = 100.0f;
	private float _lastRollTime = 100.0f;
	private float _lastBlockTime = 100.0f;

	[SerializeField] public UnityEvent _onMoveLeft;
	[SerializeField] public UnityEvent _onMoveRight;

	private void Awake()
	{
		_rigid = GetComponent<Rigidbody2D>();
		_animCtrl = gameObject.AddComponent<PlayerAnimCtrl>(); 
		_infoUIManager = gameObject.AddComponent<PlayerInfoUIManager>();
		_collMan = gameObject.AddComponent<PlayerCollisionManager>();

		_inputManager = InputManager.instance;
	}

	void Start()
	{
		_infoUIManager.UpdateHpBar(3, 3);
		_inputManager.BindInputAction("Attack", () => InputAttack(true));
		_inputManager.BindInputAction("Attack", () => InputAttack(false), true);

		_inputManager.BindInputAction("Shield", () => { _isShield = true; });
		_inputManager.BindInputAction("Shield", () => { _isShield = false; }, true);

		_inputManager.BindInputAction("Jump", () => InputJump());

		_inputManager.BindInputAction("Portal", () => InputJump());
		_inputManager.BindInputAction("Roll", () => InputRoll());

	}

	void InputAttack(bool press)
	{
		if (press && _lastAttackTime > _attackCoolTime && _isAttackable)
		{
			_isAttack = true;
		}
		else
			_isAttack = false;
	}

	void InputJump()
	{
		if (_lastJumpTime > _jumpCoolTime && _collMan._onSensorGround)
			_isJump = true;
	}

	void InputPortal()
	{
		if (_onPortalEntered != null)
		{
			if (Input.GetAxis("OnPortal") > 0.0f)
			{
				if (_onPortalEntered == null)
					return;

				_onPortalEntered.Invoke();
				_onPortalEntered = null;
			}
		}
	}
	void InputRoll()
	{
		if (_lastRollTime > _rollCoolTime && _collMan._onSensorGround)
			_isRoll = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (_isAttackable)
			_lastAttackTime += Time.deltaTime;
		_lastJumpTime += Time.deltaTime;
		_lastRollTime += Time.deltaTime;
		_lastBlockTime += Time.deltaTime;

		_xMoveDir = _inputManager.GetInputAction("Move").ReadValue<float>(); 
		
		if (UIHandler.instance.isOpenAnyUI())
			_isAttack = false; 

		if (_animCtrl.state == PlayerState.Attack == false)
		{
			if (_xMoveDir > 0)
			{
				Vector3 temp = transform.localScale;
				temp.x = Math.Abs(temp.x);
				transform.localScale = temp;
				_onMoveRight?.Invoke();
			}
			else if (_xMoveDir < 0)
			{
				Vector3 temp = transform.localScale;
				temp.x = -Math.Abs(temp.x);
				transform.localScale = temp;
				_onMoveLeft?.Invoke(); 
			}
		}

		switch (_animCtrl.state)
		{
			case PlayerState.Idle:
				OnIdle();
				break;
			case PlayerState.Run:
				OnRun();
				break;
			case PlayerState.Roll:
				OnRoll();
				break;
			case PlayerState.Attack:
				OnAttack();
				break;
			case PlayerState.IdleBlock:
				OnIdleBlock();
				break;
			case PlayerState.Block:
				OnBlock();
				break;
			case PlayerState.Fall:
			case PlayerState.WallSlider:
			case PlayerState.Jump:
				OnJump();
				break;
			case PlayerState.Hurt:
				break;
			case PlayerState.Death:
				break;
			default:
				break;
		}
	}



	private void FixedUpdate()
	{
		if (_animCtrl.state == PlayerState.Run)
		{
			float speed = _playerSpeed;

			float ySpd = _collMan._onSensorGround ? 0.0f : -2.0f;
			Vector2 curPos = _rigid.position;
			Vector2 nextPos = transform.position + new Vector3(_xMoveDir * speed, ySpd, 0.0f) * Time.fixedDeltaTime;

			_rigid.MovePosition(nextPos);


			_moveDir = nextPos - curPos;


		}
	}

	void CancelAnim()
	{
		_isAttackable = true;

	}
	void OnIdle()
	{
		_moveDir = Vector2.zero;
		if (_collMan._onSensorGround)
			_rigid.velocity = new Vector2(0.0f, 0.0f);

		if (_isShield)
		{
			_animCtrl.state = PlayerState.Block;
			return;
		}
		else if (_isRoll)
		{
			_animCtrl.state = PlayerState.Roll;
			return;
		}
		else if (_isAttack)
		{
			_animCtrl.state = PlayerState.Attack;
			return;
		}
		else if (_isJump)
		{
			_animCtrl.state = PlayerState.Jump;
			return;
		}


		if (_collMan._onSensorGround == false)
		{
			//_animCtrl.state = PlayerState.Fall;
			_rigid.AddForce(new Vector2(0.0f, -1.0f), ForceMode2D.Impulse);
			OnFalling();
			return;
		}
		if (Math.Abs(_xMoveDir) > 0)
		{
			_animCtrl.state = PlayerAnimCtrl.PlayerState.Run;
		}

	}

	void OnRoll()
	{
		if (_lastRollTime <= _rollCoolTime)
			return;

		_lastRollTime = 0.0f;

		_rigid.velocity = new Vector2(transform.localScale.x * 10.0f, 0.0f);
		_isRoll = false;
	}

	void AE_EndRoll()
	{
		_animCtrl.state = PlayerState.Idle;
		if (_lastJumpTime > _jumpCoolTime)
			_lastJumpTime = _jumpCoolTime - 0.1f;
		//rigid.velocity = Vector

	}

	void OnRun()
	{
		if (_isRoll)
		{
			_animCtrl.state = PlayerState.Roll;
			return;
		}
		if (_isAttack)
		{
			_animCtrl.state = PlayerState.Attack;
			return;
		}

		if (_isJump)
		{
			_animCtrl.state = PlayerState.Jump;
			return;
		}

		if (_collMan._onSensorGroundFar == false)
		{
			_rigid.AddForce(new Vector2(_xMoveDir * 3.0f, -1.0f), ForceMode2D.Impulse);
			OnFalling();
			return; 
		}

		if (Math.Abs(_xMoveDir) == 0)
		{
			_animCtrl.state = PlayerAnimCtrl.PlayerState.Idle;
		}

	}

	void OnAttack()
	{
		if (_lastAttackTime < _attackCoolTime) 
			return;

		_lastAttackTime = 0.0f;
		_isAttackable = false;

		int temp = _animCtrl.attackCombo + 1;
		if (temp > 3)
			temp = 1;

		_animCtrl.attackCombo = temp;
	}

	void AE_EndAttack()
	{
		_isAttackable = true;
		_animCtrl.state = PlayerState.Idle;
	} 

	 
	void AE_OnAttack()
	{   
		Collider2D[] result = new Collider2D[100];
		ContactFilter2D contactFilter = new ContactFilter2D();
		Physics2D.OverlapCollider(_collMan._sensorAttack, contactFilter, result);
		 
		foreach (var c in result) 
		{
			if (c == null)
				break; 
			 
			Monster mc = c.gameObject.GetComponent<Monster>();
			if (mc != null && mc._hp > 0)
			{
				mc.OnHit(gameObject);
				if (mc._hp == 0)
				{
					ItemManager.instance.GetItemObj(mc.gameObject.transform.position);
				//	UIHandler.instance._questManager.KillMonster(mc.gameObject);  
				}
			}
		}
	}



	void OnJump()
	{
		if (_lastJumpTime > _jumpCoolTime && _isJump)
		{
			Vector3 tv = _rigid.velocity;
			tv.y = 0.0f;
			_rigid.velocity = tv;

			_rigid.velocity += new Vector2(Math.Clamp(_moveDir.x, -1.0f, 1.0f) * _playerSpeed * 15.0f, 8.0f);
			_lastJumpTime = 0.0f;
			_isJump = false; 
		}

		OnFalling();

	}

	void OnFalling()
	{
		//Debug.Log(rigid.velocity.y);
		if (_rigid.velocity.y == 0.0f)
		{
			_animCtrl.state = PlayerState.Idle;
		}
		else if (_rigid.velocity.y < 0.0f)
		{
			_animCtrl.state = PlayerState.Fall;
			if (_collMan._onSensorFT)
			{
				OnWallSlider();
			}

			if (_collMan._onSensorGround)
			{
				_animCtrl.state = PlayerState.Idle;
				if (_lastJumpTime > _jumpCoolTime)
					_lastJumpTime = _jumpCoolTime - 0.1f;
				//Debug.Log("TO IDLE");
			}
		}
		else if (_rigid.velocity.y > 0.0f)
		{
			_animCtrl.state = PlayerState.Jump;
		}


		if (_animCtrl.state != PlayerState.WallSlider)
		{
			_rigid.AddForce(new Vector2(_xMoveDir * Time.deltaTime, 0.0f), ForceMode2D.Impulse);
		}

	}
	void OnWallSlider()
	{
		bool isAttachedLeftWall = transform.localScale.x < 0.0 && _xMoveDir < -0.1;
		bool isAttachedRightWall = transform.localScale.x > 0.0 && _xMoveDir > 0.1;

		if (isAttachedLeftWall == false && isAttachedRightWall == false)
			return;


		Vector3 t = _rigid.velocity;
		t.x = 0.0f;
		t.y = -0.1f;
		_rigid.velocity = t;
		_animCtrl.state = PlayerState.WallSlider;

		float jump = Input.GetAxis("Jump");
		if (jump > 0.0f)
		{
			if (Input.GetAxis("Vertical") > 0.5f)
				_rigid.velocity = new Vector2(0.0f, 8.0f);
			else
			{
				float dir = transform.localScale.x < 0.0f ? 1.0f : -1.0f;
				_rigid.AddForce(new Vector2(dir * 5.0f, 8.0f), ForceMode2D.Impulse);
				//rigid.velocity = new Vector2;  
			}

		}
	}

	void AE_SlideDust()
	{
		//Transform t;
		//t.position = _sensorFrontTop.gameObject.transform.position;
		 
		Instantiate(_SlideDust, _collMan._sensorFrontTop.gameObject.transform.position, new Quaternion());
	}

	void OnBlock()
	{
		if (_lastBlockTime <= _blockCoolTime)
			return;

		_lastBlockTime = 0.0f;
	}
	 
	void OnIdleBlock()
	{
		if (_isShield == false)
			_animCtrl.state = PlayerState.Idle;

	}

	void AE_EndBlock() 
	{
		if (_isShield)
		{
			_animCtrl.state = PlayerState.IdleBlock;
			return;
		}

		_animCtrl.state = PlayerState.Idle;

	}
	 

	public void OnHit(GameObject monster)
	{
		if (_animCtrl.state == PlayerState.Hurt || _animCtrl.state == PlayerState.Roll || hp == 0)  
			return;
		if (_animCtrl.state == PlayerState.Block)
		{
			Monster mst = monster.GetComponent<Monster>();
			if (mst != null)
			{
				mst._onAttackBlocked = () => { mst.OnHit(gameObject); };
				 
			}
			return;
		}

		hp--;
		_infoUIManager.UpdateHpBar(3, hp); 
		//Debug.Log(hp);
		CancelAnim(); 
		CameraManager cm = Camera.main.GetComponent<CameraManager>();

		if (monster.transform.position.x < gameObject.transform.position.x)
			cm?.ShakeCamera_Left(); 
		else
			cm?.ShakeCamera_Right(); 


		_animCtrl.state = PlayerState.Hurt;
	}

	void AE_EndHurt()
	{
		_animCtrl.state = PlayerState.Idle; 
		
		if (hp == 0)
		{
			_animCtrl.state = PlayerState.Death; 

		}
	}
}
