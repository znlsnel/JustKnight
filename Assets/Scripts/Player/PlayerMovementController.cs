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

        bool isSkillActive = false;

	private void Start() 
	{
		_playerController = GetComponent<PlayerController>();	
		_rigidbody = GetComponent<Rigidbody2D>();       
		_actionController = GetComponent<PlayerActionController>();
		_animCtrl = GetComponent<PlayerAnimCtrl>();
	}
	 

	void Update()
        {
                switch(_playerController._playerState)
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
		if (_playerController._playerState == EPlayerState.Move)
		{
			float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
			_rigidbody.velocity = new Vector2(moveDir * _playerController._playerSpeed, _rigidbody.velocity.y);


			Vector3 scale = gameObject.transform.localScale;

			if (moveDir == 0)
				_playerController._playerState = EPlayerState.Idle;
			else if (moveDir > 0)
				gameObject.transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z); 
			else
				gameObject.transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);  
		}
	}

	void OnIdleState()
        {
                float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
                if (Mathf.Abs(moveDir) > 0) 
                {
			_playerController._playerState = EPlayerState.Move;
                } 
		 
//		if (_rigidbody.velocity.y < -0.2f)
		//	_playerController._playerState = EPlayerState.Fall; 
	}

	void OnMoveState()
        {
		float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
		if (moveDir == 0)
		{
			_playerController._playerState = EPlayerState.Idle;
		}


	}

	void OnFallState()
        {
                if (_rigidbody.velocity.y >= 0)
			_playerController._playerState = EPlayerState.Idle;

		// 벽에 붙어 있을 때 애니메이션
		float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
		bool isAttachedLeftWall = transform.localScale.x < 0.0 && moveDir < -0.1; 
		bool isAttachedRightWall = transform.localScale.x > 0.0 && moveDir > 0.1;

		// 벽에 등을 대고 있다면
		if (isAttachedLeftWall == false && isAttachedRightWall == false)
			return;

		_animCtrl.PlayAnimation("Wall Slide");

		// 벽타기중 점프 누르는 로직 구현

	}



	void OnDeathState()
        {
		 
        }

}
