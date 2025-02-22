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
	PlayerStats _playerStats;

	public GameObject _SlideDust;

	[Space(10)]
	public float _attackDelay = 0.0f;
	public float _rollDelay = 0.0f;
	public float _shieldDelay = 0.0f;

	[Space(10)]
	public float _jumpPower = 5.0f;
	public float _rollPower = 5.0f;
	public float _playerSpeed = 5.0f;

	[Space(10)]
	public Transform _uiPos;

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
			_playerUI.UpdateHpBar(_hp, _playerStats.GetValue(EPlayerStatus.HP)); 
		}
	}


	public UnityEvent qr_MoveLeft;
	public UnityEvent qr_MoveRight;
	public UnityEvent qr_Jump;
	public UnityEvent qr_wallJump; 
	public UnityEvent qr_roll;
	public UnityEvent qr_attack;
	public UnityEvent qr_shield;
	public UnityEvent qr_successShield;

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
		_actionController = gameObject.GetComponent<PlayerActionController>();
		_movementController = gameObject.AddComponent<PlayerMovementController>();

		_playerUI = UIHandler.instance._playerUI.GetComponent<PlayerUI>();
		_playerUI.gameObject.SetActive(true);

		_inputManager = InputManager.instance;

	}
	 
	void Start()
	{
		_collMan = gameObject.GetComponent<PlayerCollisionManager>();
		_playerStats = PlayerStats.instance;
		
		
		_hp = _playerStats.GetValue(EPlayerStatus.HP);    
		_playerUI.UpdateHpBar(_hp, _hp);
	}

	void AE_SlideDust()
	{
		Instantiate(_SlideDust, _collMan._sensorFrontTop.gameObject.transform.position, new Quaternion());
	}

	public void InitCharacter(Vector3 pos, int hp)
	{
		this.hp = hp;
		transform.position = pos;
	}


} 
