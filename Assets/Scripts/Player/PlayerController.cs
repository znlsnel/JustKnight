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
	PlayerAnimCtrl _animController;
	PlayerInfoUIManager _infoUIManager;
	PlayerCollisionManager _collMan; 
	InputManager _inputManager;
	PlayerActionController _actionController;
	PlayerMovementController _movementController;

	public float _attackDelay = 0.0f;
	public float _rollDelay = 0.0f;
	public float _shieldDelay = 0.0f;

	[SerializeField] public float _playerSpeed = 5.0f; 

	[SerializeField] public GameObject _SlideDust;


	public int hp = 3;
	public int InitHp = 3;

	[SerializeField] public UnityEvent r_MoveLeft;
	[SerializeField] public UnityEvent r_MoveRight;
	[SerializeField] public UnityEvent r_Jump;
	[SerializeField] public UnityEvent r_wallJump; 
	[SerializeField] public UnityEvent r_roll;
	[SerializeField] public UnityEvent r_attack;
	[SerializeField] public UnityEvent r_shield;
	[SerializeField] public UnityEvent r_successShield;

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
		_infoUIManager = gameObject.AddComponent<PlayerInfoUIManager>();
		_actionController = gameObject.AddComponent<PlayerActionController>(); 
		_movementController = gameObject.AddComponent<PlayerMovementController>();
		_inputManager = InputManager.instance;
	}

	void Start()
	{
		_collMan = gameObject.GetComponent<PlayerCollisionManager>();  
		_infoUIManager.UpdateHpBar(3, 3);

	}

	void AE_SlideDust()
	{
		Instantiate(_SlideDust, _collMan._sensorFrontTop.gameObject.transform.position, new Quaternion());
	}



	
} 
