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
	[SerializeField] float _playerSpeed = 5.0f;

	Rigidbody2D rigid; 
	PlayerAnimCtrl _animCtrl;
	  
	Vector2 _moveDir = Vector2.zero;

	 float _xMoveDir = 0.0f;

	bool _isAttack = false;
	bool _isJump = false;
	bool _onSensorFrontTop = false;
	bool _onSensorFrontBt = false; 
	bool _onSensorBackBt = false;
	bool _onSensorGround = false; 

	private float _lastAttackTime = 100.0f;
	private float _lastJumpTime = 100.0f;

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
		_lastAttackTime += Time.deltaTime;
		_lastJumpTime += Time.deltaTime;
		 
		_xMoveDir = Input.GetAxis("Horizontal");
		_isAttack = Input.GetAxis("Fire1") > 0.9f && _lastAttackTime > _attackCoolTime;
		_isJump = Input.GetAxis("Jump") > 0.9f && _lastJumpTime > _jumpCoolTime && _onSensorGround;

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
				break;
			case PlayerState.Attack:
				OnAttack();
				break;
			case PlayerState.IdleBlock:
				break;
			case PlayerState.Block:
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

		if (Math.Abs(_xMoveDir) > 0)
		{
			_animCtrl.state = PlayerAnimCtrl.PlayerState.Run;
		}

	}

	void OnRun()
	{
		 
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


		if (_attackCoolTime < 0.7f)
			_animCtrl.anim.speed = 0.7f / _attackCoolTime;    
		  
		int temp = _animCtrl.attackCombo + 1;
		if (temp > 3)
			temp = 1;

		_animCtrl.attackCombo = temp;
	}
	  
	void EndAttack()
	{
		_animCtrl.anim.speed = 1.0f;
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
			//Vector2 t = new Vector2(Math.Clamp(_moveDir.x, -1.0f, 1.0f) * _playerSpeed * 15.0f , 8.0f);
			//rigid.AddForce(t, ForceMode2D.Impulse); 
			//rigid.AddForce(new Vector2(_moveDir.x * _playerSpeed, 8.0f)); 
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
}
