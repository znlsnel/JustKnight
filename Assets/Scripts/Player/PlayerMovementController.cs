using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState
{
        Idle,
        Move,
        Fall,
        Death
};

public class PlayerMovementController : MonoBehaviour
{
        // Start is called before the first frame update
	Rigidbody2D _rigidbody;
        PlayerController _playeManager;
        [NonSerialized] public EPlayerState _playerState = EPlayerState.Idle;

        bool isSkillActive = false;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();       
	}

        void Update()
        {
                switch(_playerState)
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
		if (_playerState == EPlayerState.Move)
		{
			float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
			_rigidbody.velocity = new Vector2(moveDir * _playeManager._playerSpeed, _rigidbody.velocity.y);

			if (moveDir == 0)
				_playerState = EPlayerState.Idle;
		}
	}

	void OnIdleState()
        {
                float moveDir = InputManager.instance.GetInputAction("Move").ReadValue<float>();
                if (Mathf.Abs(moveDir) > 0) 
                {
                        _playerState = EPlayerState.Move;
                }

		if (_rigidbody.velocity.y < 0.2f)
                        _playerState = EPlayerState.Fall;
	}

	void OnMoveState()
        {
		if (_rigidbody.velocity.y < 0.2f)
			_playerState = EPlayerState.Fall;
	}

	void OnFallState()
        {
                if (_rigidbody.velocity.y >= 0)
                        _playerState = EPlayerState.Idle;
        }

	void OnDeathState()
        {

        }

}
