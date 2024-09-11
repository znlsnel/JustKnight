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
	PlayerStats _stats;

	bool _isWallClimb = false;

	int characterLookDir;
	int _characterLookDir 
	{ 
		get 
		{ 
			return characterLookDir; 
		}
		set
		{
			if (value == characterLookDir)
				return;
			 
			characterLookDir = value;
			_onPlayerLookDirChanged?.Invoke(characterLookDir); 
		} 
	}
	public Action<int> _onPlayerLookDirChanged;


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
		
		if (_playerController._playerState == EPlayerState.Death || _actionController._activeState == EActiveState.Hit || moveDir == 0.0f)
			return;
		  
		if (_playerController._playerState == EPlayerState.Move && _actionController._activeState != EActiveState.Roll)
		{
			float moveSize = _playerController._playerSpeed * (float)PlayerStats.instance.GetValue(EPlayerStatus.MoveSpeed) * 0.01f;
			_rigidbody.velocity = new Vector2(moveDir * moveSize, _rigidbody.velocity.y);

			if (moveDir > 0)
				_playerController.qr_MoveRight.Invoke();
			else if (moveDir < 0)
				_playerController.qr_MoveLeft.Invoke();
		} 

		else if (!_playerCollision._onSensorGround && _actionController._activeState != EActiveState.Roll)
		{
			if (!_isWallClimb || !_playerCollision._onSensorFT)   
			{    
				gameObject.transform.position += new Vector3(moveDir * Time.fixedDeltaTime, 0.0f, 0.0f);
			}
		}   
		 
		if (_actionController._activeState != EActiveState.Shield && _playerController._playerState != EPlayerState.Death)
		{
			_characterLookDir = moveDir > 0 ? 1 : -1;
			Vector3 scale = gameObject.transform.localScale;
			gameObject.transform.localScale = new Vector3(Mathf.Abs(scale.x) * _characterLookDir, scale.y, scale.z); 
		}
		
		// 공중에 있을 때에도 약간의 이동이 되야하면서
		// 벽타기 -> 점프에는 영향이 없어야함
		
	}
	void OnIdleState()
	{
		if (_actionController._activeState == EActiveState.Roll ||
			_actionController._activeState == EActiveState.Attack ||  
			_actionController._activeState == EActiveState.Hit)
			return;   
		  
                float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>(); 
                if (Mathf.Abs(moveDir) > 0 && _actionController._activeState != EActiveState.Jump)  
			_playerController._playerState = EPlayerState.Move;
                
		if (_rigidbody.velocity.y < -0.2f && !_playerCollision._onSensorGround)
			_playerController._playerState = EPlayerState.Fall;

		 
		Vector3 vel = _rigidbody.velocity; 
		_rigidbody.velocity = new Vector3(vel.x - (vel.x * Time.deltaTime), vel.y, vel.z);   
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
			_isWallClimb = false;
			return;
		}
		 
		// 벽 타기
		bool isMoving = Mathf.Abs(InputManager.instance.GetInputAction("Move").ReadValue<float>()) > 0;
		if (_playerCollision._onSensorFT && isMoving)
		{ 
			_rigidbody.velocity = new Vector3(0.0f, -0.2f, 0.0f);

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
					_isWallClimb = true;
				}
				// 옆 점프
				else
				{
					_isWallClimb = false;
					_actionController.OnJump(true);
					_playerController.qr_wallJump.Invoke(); 
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
