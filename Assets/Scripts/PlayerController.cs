using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimCtrl;

public class PlayerController : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] float _attackCoolTime = 0.2f;
	//[SerializeField] float _postAttackDelay = 0.7f;
	[SerializeField] float _jumpCoolTime = 0.2f;

	Rigidbody2D rigid; 
	PlayerAnimCtrl _animCtrl;
	[SerializeField] float _playerSpeed = 5.0f;
	  
	float _xMoveDir = 0.0f;
	bool _isAttack = false;
	bool _isJump = false;

	Vector2 _moveDir = Vector2.zero;
	private float _lastAttackTime = 100.0f;
	private float _lastJumpTime = 100.0f;

	private void Awake() 
	{
		_animCtrl = GetComponent<PlayerAnimCtrl>();
		rigid = GetComponent<Rigidbody2D>();
		
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
		_isJump = Input.GetAxis("Jump") > 0.9f && _lastJumpTime > _jumpCoolTime && rigid.velocity.y == 0;

		  
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

		//transform.position += new Vector3(_xMoveDir * _playerSpeed, 0.0f, 0.0f) * Time.deltaTime; 
		Vector2 curPos = rigid.position;
		Vector2 nextPos = curPos + new Vector2(_xMoveDir * _playerSpeed, 0.0f) * Time.deltaTime;
		rigid.MovePosition(nextPos);
		_moveDir = nextPos - curPos; 
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
		Debug.Log(" END ATTACK");
	}

	  
	void OnJump()
	{
		if (_lastJumpTime > _jumpCoolTime && _isJump)
		{
			rigid.velocity += new Vector2(_moveDir.x * _playerSpeed, 10.0f); 

			_lastJumpTime = 0.0f; 
		} 

		if (rigid.velocity.y == 0.0f)
		{
			_animCtrl.state = PlayerState.Idle; 
		}
		else if (rigid.velocity.y < 0.0f)
		{
			_animCtrl.state = PlayerState.Fall;
		}
		else if (rigid.velocity.y > 0.0f)
		{
			_animCtrl.state = PlayerState.Jump;
		}

	} 
}
