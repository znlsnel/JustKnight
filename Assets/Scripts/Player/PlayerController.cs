using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static PlayerAnimCtrl;
public enum EPlayerState
{
	Idle,
	Move,
	Fall,
	Death
};
public class PlayerController : MonoBehaviour
{
	PlayerUI _playerUI;
	PlayerAnimCtrl _animController;
	PlayerCollisionManager _collMan; 
	InputManager _inputManager;
	PlayerActionController _actionController;
	PlayerMovementController _movementController;
	PlayerEffectManager _effectManager;

	public GameObject _SlideDust;

	[Space(10)]
	public float _attackDelay = 0.0f;
	public float _rollDelay = 0.0f;
	public float _shieldDelay = 0.0f;

	[Space(10)]
	public float _jumpPower = 5.0f;
	public float _rollPower = 5.0f;
	public float _playerSpeed = 5.0f; 
	public int _InitHp = 3;

	int _hp = 3;
	public int hp 
	{ 
		get 
		{ 
			return _hp;
		} 
		set 
		{ 
			_hp = value;
			_playerUI.UpdateHpBar(_hp, _InitHp); 
		}
	}


	public UnityEvent r_MoveLeft;
	public UnityEvent r_MoveRight;
	public UnityEvent r_Jump;
	public UnityEvent r_wallJump; 
	public UnityEvent r_roll;
	public UnityEvent r_attack;
	public UnityEvent r_shield;
	public UnityEvent r_successShield;

	private EPlayerState _state = EPlayerState.Idle;
	public EPlayerState _playerState
	{
		get { return _state; }
		set
		{
			if (_state == value)
				return;

			_state = value;
			_animController.PlayAnimation();

			if (_state == EPlayerState.Fall)
				_actionController.OnFallStart(); 
			 
		}
	} 

	private void Awake() 
	{
		_animController = gameObject.AddComponent<PlayerAnimCtrl>(); 
		_actionController = gameObject.AddComponent<PlayerActionController>();
		_movementController = gameObject.AddComponent<PlayerMovementController>();
		_inputManager = InputManager.instance;
		_hp = _InitHp; 
	}
	 
	void Start()
	{
		_collMan = gameObject.GetComponent<PlayerCollisionManager>();
		_effectManager = PlayerEffectManager.instance;
		_playerUI = UIHandler.instance._playerUI.GetComponent<PlayerUI>();
		_playerUI.gameObject.SetActive(true);
		_playerUI.UpdateHpBar(_hp, _InitHp); 
	}

	void AE_SlideDust()
	{
		Instantiate(_SlideDust, _collMan._sensorFrontTop.gameObject.transform.position, new Quaternion());
	}



	
} 
