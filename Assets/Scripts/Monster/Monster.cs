using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour 
{
	// Start is called before the first frame update

	protected Animator _animator;
	protected GameObject _player;
	protected Rigidbody2D _rigid;
	protected Collider2D _frontCollisionSensor;
	

	protected enum MonsterState
        { 
                Idle,

                Move, 

                Attack,

                Death,

                Hit,
        } 
	protected MonsterState _state = MonsterState.Idle;
         
        [SerializeField] protected float _moveSpeed = 1.0f;
	[SerializeField] protected float _attackCoolTime = 1.0f;
        [SerializeField] protected float _trackingDist = 5.0f;
         
	protected bool _isChasing = false;

	protected bool _onFrontCollisionSensor = false;
        protected bool _isInPlayerAttackRange = false;

	protected float _trackingIdleTime = 1.0f; 
	protected float _trackingMoveTime = 2.0f; 

	protected float _lastIdleTime = 0.0f;
	protected float _lastMoveTime = 0.0f;
	protected float _lastAttackTime = 0.0f;

	[SerializeField] GameObject _hpUI;
	Slider _hpSlider;

	public int _hp = 3;
        public int _initHp = 3;
	public Action _onAttackBlocked;
	[SerializeField] public UnityEvent _onDead;

	public virtual void Awake()
	{
		_hp = _initHp; 
		if (_hpUI != null)
		{
			_hpUI = Instantiate<GameObject>(_hpUI); 

			_hpSlider = _hpUI.GetComponentInChildren<Slider>();
		}
 
	}
	public virtual void InitMonster(Vector3 pos)
        {
		gameObject.SetActive(true); 
		_state = MonsterState.Idle;
		gameObject.transform.position = pos;
		
		if(_animator != null)
			_animator.speed = 1.0f; 
		_hp = _initHp; 
		//Debug.Log($"HP : {_hp} , InitHp : {_initHp}"); 
	}
          
     
        public virtual void Start()
        { 
                _animator = GetComponent<Animator>();
		_player = GameManager.instance.GetPlayer();
		_rigid = GetComponent<Rigidbody2D>();
		_frontCollisionSensor = transform.Find("CollisionSensor_Front").GetComponent<Collider2D>(); 
	}

        const float SensorUpdateCycle = 1.0f / 30.0f;
        float lastSensorUpdateTime = 0.0f;
        protected void UpdateSensor()
        {
		lastSensorUpdateTime += Time.deltaTime;
		if (lastSensorUpdateTime < SensorUpdateCycle)
                        return; 
                 
                lastSensorUpdateTime = 0.0f;

		_onFrontCollisionSensor = false;
                _isInPlayerAttackRange = false;

	        ContactFilter2D ft = new ContactFilter2D();
                Collider2D[] colliders = new Collider2D[4]; 
                 int count = Physics2D.OverlapCollider(_frontCollisionSensor, ft, colliders);
                  
		for (int i = 0; i < count; i++) { 
                        if (colliders[i].gameObject != gameObject)
                        {  
                                if (!_onFrontCollisionSensor)
                                         _onFrontCollisionSensor = colliders[i].GetComponent<Monster>() == null;

                                if (!_isInPlayerAttackRange && colliders[i].gameObject == _player)
                                {
                                        _isInPlayerAttackRange = colliders[i].gameObject.GetComponent<PlayerAnimCtrl>()?.state != PlayerAnimCtrl.PlayerState.Death;
				} 
                                  
			}
                }
	}
        
         
        public virtual void Update()
        {
                UpdateSensor(); 
                
                Vector3 MonsterToPlayer = _player.transform.position - transform.position;  
                float DistanceToPlayer = MonsterToPlayer.magnitude;
		
                // 거리가 5보다 작고 플레이어쪽을 바라보고 있다면 추적 시작
                if (DistanceToPlayer < _trackingDist && MonsterToPlayer.x * transform.localScale.x > 0.0f)
                        _isChasing = true;
		 
                // 거리가 10보다 커졌거나 플레이어가 죽었다면 추적 종료
                if (DistanceToPlayer > _trackingDist * 2.0f || _player.GetComponent<PlayerAnimCtrl>()?.state == PlayerAnimCtrl.PlayerState.Death)
                        _isChasing = false;

                // 추적중이라면 플레이어를 바라보게 설정
                float dist = Math.Abs(MonsterToPlayer.x); 
                if (_isChasing && _state == MonsterState.Move && dist > 0.1f) 
                {
                        Vector3 t = transform.localScale;
                        float tx = Math.Abs(t.x);
                        t.x = MonsterToPlayer.x > 0.0f ? tx : -tx;
                        transform.localScale = t;   
                        
                }

                UpdateState();
        }  
         
	protected virtual void FixedUpdate()
	{
                bool movable = _state == MonsterState.Move && _onFrontCollisionSensor == false;
                
		if (movable)
                {   
                        Vector2 nextPos = transform.position + new Vector3(transform.localScale.x * _moveSpeed, -1.0f, 0.0f) * Time.fixedDeltaTime;
			_rigid.MovePosition(nextPos);  
		}
	}

        public virtual void UpdateState()
        {
		switch (_state)
		{
			case MonsterState.Idle:
				OnIdle();
				_animator.Play("Idle");
				break;
			case MonsterState.Move:
				OnMove();
				_animator.Play("Walk");
				break;
			case MonsterState.Attack:
				OnAttack();
				break;
			case MonsterState.Hit:
				_animator.Play("Hit"); 
				break;
			case MonsterState.Death:
				_animator.Play("Death");
				OnDeath();
				break;
			default:
				break;
		}

	}


	public virtual void OnIdle()
        {
		if (_isChasing)
		{
			bool isClosePlayer = Math.Abs(_player.transform.position.x - transform.position.x) < 0.1f;

			if (_isInPlayerAttackRange)
				_state = MonsterState.Attack;

			else if (isClosePlayer == false)
				_state = MonsterState.Move;

			return;
		}

		_lastIdleTime += Time.deltaTime;
		if (_lastIdleTime < _trackingIdleTime)
			return;

		_lastIdleTime = 0.0f;
		_state = MonsterState.Move;

		int nextDir = _onFrontCollisionSensor ? -1 : (UnityEngine.Random.Range(0, 2) * 2) - 1;

		Vector3 ts = transform.localScale;
		ts.x *= nextDir;
		transform.localScale = ts;
	}
	public virtual void OnMove()
        {
		if (_isChasing == false)
		{

			_lastMoveTime += Time.deltaTime;
			if (_lastMoveTime < _trackingMoveTime && _onFrontCollisionSensor == false)
				return;
			_lastMoveTime = 0.0f;

			_state = MonsterState.Idle;
			return;
		}


		bool isClosePlayer = Math.Abs(_player.transform.position.x - transform.position.x) < 0.1f;
		if (_isInPlayerAttackRange)
		{
			_animator.Play("Idle");
			_state = MonsterState.Attack;
		}
		else if (isClosePlayer)
		{
			_state = MonsterState.Idle;
		}

	}

	public abstract void OnAttack();
          
        public void OnHit(GameObject attacker)
        {
                if (_hp == 0)
                        return;

		_hp -= 1;
		if (_hpSlider != null) 
			_hpSlider.value = (float)_hp / _initHp;  
		_state = MonsterState.Hit;
                _isChasing = true;
	} 

        void AE_EndHit()
        {
                _lastAttackTime = _attackCoolTime;
                 
                if (_hp == 0)
                        _state = MonsterState.Death;
                else
                        _state = MonsterState.Idle;
                 
	}
        void AE_EndDeath()
        {
		_animator.speed = 0.0f;
		_onDead?.Invoke();
		
	}

        void AE_OnAttack()
        {
                if (_isInPlayerAttackRange == false)
                        return;
                 
                 _player.GetComponent<PlayerController>()?.OnHit(gameObject); 

        }

	
	void AE_OnAttackBlocked()
	{
		if (_onAttackBlocked != null)
		{
			_lastAttackTime = 0.0f;
			_onAttackBlocked.Invoke();
			_onAttackBlocked = null;	
		}
	}


	void AE_EndAttack()
        { 
		_lastAttackTime = 0.0f;
		_animator.Play("Idle");
	}


        public abstract void OnDeath();

}
