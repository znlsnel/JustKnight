using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using static PlayerAnimCtrl;

public class PlayerController : MonoBehaviour
{

	CircleCollider2D _sensorFrontTop;
	CircleCollider2D _sensorFrontBottom; 
	CircleCollider2D _sensorBackBottom;  
	CircleCollider2D _sensorGround; 

	[SerializeField] float _attackCoolTime = 0.2f;
	[SerializeField] float _jumpCoolTime = 0.2f;
	[SerializeField] float _rollCoolTime = 1.0f;
	[SerializeField] float _blockCoolTime = 1.0f;

	[SerializeField] float _playerSpeed = 5.0f;

	[SerializeField] GameObject _SlideDust; 

	Rigidbody2D rigid; 
	PlayerAnimCtrl _animCtrl;
	  
	Vector2 _moveDir = Vector2.zero;

	 float _xMoveDir = 0.0f;

	bool _onSensorFrontTop = false;
	bool _onSensorFrontBt = false; 
	bool _onSensorBackBt = false; 
	bool _onSensorGround = false;

	bool _isAttack = false; 
	bool _isJump = false;
	bool _isRoll = false;
	bool _isBlock = false;

	bool _isAttackable = true;
	 
	private float _lastAttackTime = 100.0f; 
	private float _lastJumpTime = 100.0f;
	private float _lastRollTime = 100.0f;
	private float _lastBlockTime = 100.0f;

	private void Awake() 
	{
		_animCtrl = GetComponent<PlayerAnimCtrl>();
		rigid = GetComponent<Rigidbody2D>();
		_sensorFrontTop = transform.Find("CollisionSensor_FrontTop").GetComponent<CircleCollider2D>();
		_sensorFrontBottom = transform.Find("CollisionSensor_FrontBottom").GetComponent<CircleCollider2D>();
		_sensorBackBottom = transform.Find("CollisionSensor_BackBottom").GetComponent<CircleCollider2D>();
		_sensorGround = transform.Find("CollisionSensor_Ground").GetComponent<CircleCollider2D>();
	}

	void Start()
	{
                
	}  

    // Update is called once per frame
    void Update()
    {
		if (_isAttackable) 
			_lastAttackTime += Time.deltaTime;
		_lastJumpTime += Time.deltaTime;
		_lastRollTime += Time.deltaTime;
		_lastBlockTime += Time.deltaTime; 
		 
		_xMoveDir = Input.GetAxis("Horizontal");
		_isAttack = Input.GetAxis("Fire1") > 0.9f && _lastAttackTime > _attackCoolTime && _isAttackable;
		_isJump = Input.GetAxis("Jump") > 0.9f && _lastJumpTime > _jumpCoolTime && _onSensorGround;
		_isRoll = Input.GetAxis("Roll") > 0.9f && _lastRollTime > _rollCoolTime && _onSensorGround;
		_isBlock = Input.GetAxis("Block") > 0.9f; 
		   
		CheckSensor();

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
			float ySpd = _onSensorGround ? 0.0f : -2.0f; 
			Vector2 curPos = rigid.position;
			Vector2 nextPos = transform.position + new Vector3(_xMoveDir * _playerSpeed, ySpd, 0.0f) * Time.fixedDeltaTime;

			rigid.MovePosition(nextPos); 


			_moveDir = nextPos - curPos;
			

		}
	}
	void CheckSensor()
	{
		ContactFilter2D contactFilter = new ContactFilter2D();

		Func<CircleCollider2D, bool> check = (CircleCollider2D collider ) =>
		{
			bool returnVal = false;
			Collider2D[] result = new Collider2D[2];
			int count = Physics2D.OverlapCollider(collider, contactFilter, result); 
			for (int i = 0; i < count; i++)
			{
				returnVal = result[i].gameObject != gameObject;

				if (returnVal)  
					break;
			}
			return returnVal; 
		};

		_onSensorFrontTop = check(_sensorFrontTop);
		_onSensorFrontBt = check(_sensorFrontBottom); 
		_onSensorBackBt = check(_sensorBackBottom);
		_onSensorGround = check(_sensorGround);
	} 
	
	void OnIdle()
	{
		_moveDir = Vector2.zero;
		rigid.velocity = new Vector2(0.0f, 0.0f); 

		if (_isBlock)
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

		rigid.velocity = new Vector2(transform.localScale.x * 10.0f, 0.0f);  
		 
	}
	 
	void AE_EndRoll()
	{
		_animCtrl.state = PlayerState.Idle;
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



		 
		if (Math.Abs(_xMoveDir) == 0)
		{
			_animCtrl.state = PlayerAnimCtrl.PlayerState.Idle;
		}



		if (_xMoveDir > 0)
		{
			Vector3 temp = transform.localScale;
			temp.x = Math.Abs(temp.x);
			transform.localScale = temp;
		}

		else if (_xMoveDir < 0)
		{
			Vector3 temp = transform.localScale;
			temp.x = -Math.Abs(temp.x);
			transform.localScale = temp;
		}
		 
		//Vector2 curPos = rigid.position; 
		//Vector2 nextPos = transform.position + new Vector3(_xMoveDir * _playerSpeed, 0.0f, 0.0f) * Time.deltaTime;
		//rigid.MovePosition(nextPos); 

		//_moveDir = nextPos - curPos; 
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

	  
	void OnJump()
	{

		if (_lastJumpTime > _jumpCoolTime && _isJump) 
		{
			Vector3 tv = rigid.velocity;
			tv.y = 0.0f;
			rigid.velocity = tv; 
			  
			rigid.velocity += new Vector2(Math.Clamp(_moveDir.x, -1.0f, 1.0f) * _playerSpeed * 15.0f , 8.0f);  
			_lastJumpTime = 0.0f; 
		} 

		if (rigid.velocity.y == 0.0f)
		{
			_animCtrl.state = PlayerState.Idle;
		}
		else if (rigid.velocity.y < 0.0f)
		{
			_animCtrl.state = PlayerState.Fall;
			if (_onSensorFrontTop)
			{
				_animCtrl.state = PlayerState.WallSlider;  
				Vector3 t = rigid.velocity;
				t.x = 0.0f;
				t.y *= 0.8f; 
				rigid.velocity = t; 
			}

			if (_onSensorGround)
			{
				_animCtrl.state = PlayerState.Idle;
				Debug.Log("TO IDLE");
			}
		}
		else if (rigid.velocity.y > 0.0f)
		{
			_animCtrl.state = PlayerState.Jump;
		}

	} 

	 
	void AE_SlideDust()
	{
		//Transform t;
		//t.position = _sensorFrontTop.gameObject.transform.position;

		Instantiate(_SlideDust, _sensorFrontTop.gameObject.transform.position, new Quaternion()); 
	}

	void OnBlock()
	{
		if (_lastBlockTime <= _blockCoolTime)
			return;

		_lastBlockTime = 0.0f; 
	}

	void OnIdleBlock()
	{
		 if (_isBlock == false)
			_animCtrl.state = PlayerState.Idle;
		  
	}

	void AE_EndBlock()
	{
		if (_isBlock)
		{
			_animCtrl.state = PlayerState.IdleBlock;
			return;
		}

		_animCtrl.state = PlayerState.Idle;
		 
	}
}
