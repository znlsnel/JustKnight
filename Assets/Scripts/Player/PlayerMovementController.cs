using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovementController : MonoBehaviour
{
        // Start is called before the first frame update
	public Rigidbody2D _rigidbody;
        PlayerController _playerController;
	PlayerAnimCtrl _animCtrl;
	PlayerActionController _actionController;
	PlayerCollisionManager _playerCollision;

	private void Start() 
	{
		_playerController = GetComponent<PlayerController>();	
		_rigidbody = GetComponent<Rigidbody2D>();       
		_actionController = GetComponent<PlayerActionController>();
		_animCtrl = GetComponent<PlayerAnimCtrl>();
		_playerCollision = GetComponent<PlayerCollisionManager>();
	}
	 

	void Update()
        { 
		//if (!_playerCollision._onSensorGround)
		//	Debug.Log("공중");  

		switch (_playerController._playerState)
                {
                        case EPlayerState.Idle:
                                OnIdleState();
				break;

                        case EPlayerState.Move:
				OnMoveState();
				break; 

                        case EPlayerState.Fall:
				OnFallState();
				break;

                        case EPlayerState.Death:
                                OnDeathState();
				break;
                }
        }
	private void FixedUpdate()
	{
		UpdateMovement(); 
	}

	void UpdateMovement()
	{ 

		float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();

		if (moveDir == 0.0f)
			return;

		EPlayerState pState = _playerController._playerState;
		EActiveState acState = _actionController._activeState;

		bool isWallAttached = !_playerCollision._onSensorGround && (_playerCollision._onSensorFT || _playerCollision._onSensorFB);   
		if (!isWallAttached && pState != EPlayerState.Death && acState != EActiveState.Roll)
		{
			float speed = _playerController._playerSpeed;
			if (!_playerCollision._onSensorGround && Mathf.Abs(_rigidbody.velocity.x) < 1.0f)
				speed = 1.0f;

			_rigidbody.velocity = new Vector2(moveDir * speed, _rigidbody.velocity.y);
		} 

		if (acState != EActiveState.Shield && pState != EPlayerState.Death)
		{
			int dir = moveDir > 0 ? 1 : -1;
			Vector3 scale = gameObject.transform.localScale;
			gameObject.transform.localScale = new Vector3(Mathf.Abs(scale.x) * dir, scale.y, scale.z);
		}
		
	}
	void OnIdleState()
	{
		if (_actionController._activeState == EActiveState.Roll)
			return;  
		  
                float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
                if (Mathf.Abs(moveDir) > 0 && _actionController._activeState != EActiveState.Jump)  
			_playerController._playerState = EPlayerState.Move;
                
		if (_rigidbody.velocity.y < -0.2f && !_playerCollision._onSensorGround)
			_playerController._playerState = EPlayerState.Fall;
	} 

	void OnMoveState()
        {
		float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
		if (moveDir == 0)
			_playerController._playerState = EPlayerState.Idle;
		 
		if (_rigidbody.velocity.y < -0.2f && !_playerCollision._onSensorGround)
			_playerController._playerState = EPlayerState.Fall;
	}

	Coroutine slideDustCT;
	void OnFallState()  
        {
                if (_playerCollision._onSensorGround)
		{
			_playerController._playerState = EPlayerState.Idle;
			_actionController.OnLand(); 
			return;
		}
		
		bool isMoving = Mathf.Abs(InputManager.instance.GetInputAction("Move").ReadValue<float>()) > 0;
		if (_playerCollision._onSensorFT && isMoving)
		{ 
			Vector3 vel = _rigidbody.velocity;
			vel.y = -0.2f;
			_rigidbody.velocity = vel;

			if (slideDustCT == null)
				slideDustCT = StartCoroutine(UseSlideDust());
			_animCtrl.PlayAnimation("Wall Slide");

			// 벽 점프
			if (InputManager.instance.GetInputAction("Jump").IsPressed())
			{
				// 위 점프
				if (InputManager.instance.GetInputAction("Up").IsPressed())
				{ 
					_actionController.OnJump(); 
				}
				// 옆 점프
				else
				{
					_actionController.OnJump(true);
				}

			}

		}
		else
			_animCtrl.PlayAnimation();
	}

	IEnumerator UseSlideDust()
	{
		GameObject.Instantiate<GameObject>(_playerController._SlideDust, _playerCollision._sensorFrontTop.gameObject.transform);
		yield return new WaitForSeconds(1.0f);

		bool isMoving = Mathf.Abs(InputManager.instance.GetInputAction("Move").ReadValue<float>()) > 0;
		if (_playerController._playerState == EPlayerState.Fall && _playerCollision._onSensorFT && isMoving)
			StartCoroutine(UseSlideDust()); 
		else
		{
			slideDustCT = null; 
	
		}
	}


	void OnDeathState()
        {
		 
        }

}
