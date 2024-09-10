using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum MonsterState
{
	Waiting,
	Move,
	Chasing,
	Attack,
	Death,
}

[Serializable]
public struct DropItem
{
	public ItemSO item;
	public int dropRate; 
}
public abstract class Monster : MonoBehaviour
{
	// Start is called before the first frame update 

	protected GameObject _player;
	protected Rigidbody2D _rigid;
	protected Animator _animator;
	protected MonsterState _state = MonsterState.Waiting;
	protected bool _isPlayerInAttackRange { get { return CheckSensor(_attackSensor, "Player"); } }
	protected bool _isObstacleAhead { get { return CheckSensor(_wallSensor) || !CheckSensor(_groundSensor); } }

	public Collider2D _wallSensor;
	public Collider2D _groundSensor;
	public Collider2D _attackSensor;

	[Space(10)]
	public UnityEvent _onDead;
	public GameObject _hpUI;
	public Transform _hpPos;
	public GameObject _damageUIPrefab;
	DamageUI _damageUI;
	[SerializeField] MonsterEffect _effect;

	[Space(10)]
	public float _moveSpeed = 1.0f;
	public float _attackCoolTime = 1.0f;
	public float _attackRange = 3.0f;
	public float _waitingTime = 1.0f;
	public float _moveTime = 2.0f;
	public float _playerTrackableRange = 3.0f;

	[Space(10)]
	int _hp = 3;
	public int monsterHp {get {return _hp;}} 
	public int _initHp = 3;
	public List<DropItem> _dropItems;

	[Space(10)]
	[SerializeField] UnityEvent _onDamaged;
	public int AttackCnt = 1;

	float _lastwaitTime = 0.0f;
	float _lastMoveTime = 0.0f;
	protected Slider _hpSlider;
	protected float _lastAttackTime = 0.0f;

	protected bool isHit = false;
	protected bool isAttack = false;
	protected bool isPlayerDead
	{
		get
		{
			return _player.GetComponent<PlayerController>()._playerState == EPlayerState.Death;
		}
	}

	public Action _onAttackBlocked;
	public Action _onDestroy;


	[NonSerialized] public int curAttackAnim = 1;

	 
	public virtual void Awake()
	{
		_hp = _initHp; 
		if ( _hpUI != null ) 
		{  
			if (isPrefab(_hpUI))
				_hpUI = Instantiate<GameObject>(_hpUI);  
			_hpSlider = _hpUI.GetComponentInChildren<Slider>();
			
		} 
		_animator = GetComponent<Animator>();
		_rigid = GetComponent<Rigidbody2D>();
		_damageUIPrefab = Instantiate<GameObject>(_damageUIPrefab);
		_damageUI = _damageUIPrefab.GetComponent<DamageUI>();
		_damageUI._parent = _hpSlider.gameObject;
	} 

	public virtual void InitMonster(Vector3 pos)
        {
		gameObject.transform.position = new Vector3(pos.x, pos.y + transform.localScale.y / 2, pos.z);
		_state = MonsterState.Waiting;

		_animator.speed = 1.0f; 
		_hp = _initHp; 
		 
		if (_hpSlider != null ) 
			_hpSlider.value = (float)_hp / _initHp; 
		
		 
		gameObject.SetActive(true);
		_hpUI.SetActive(true);
	}


	public virtual void Start()
        { 
		_player = GameManager.instance.GetPlayer();
	}

	bool isPrefab(GameObject obj)
	{
		return obj.scene.rootCount == 0;
	}
        protected bool CheckSensor(Collider2D sensor, string findLayer = "")
        {
		
                Collider2D[] colliders = new Collider2D[10]; 
                Physics2D.OverlapCollider(sensor, new ContactFilter2D(), colliders);
                  
		foreach (Collider2D collider in colliders)  
		{
			if (collider == null)
				break;
			 
			if (findLayer != "" && collider.gameObject.layer != LayerMask.NameToLayer(findLayer))
				continue;

                        if (collider.GetComponent<Monster>() == null)
				return true;                                  
                } 
		 
		return false;
	}
        
         
        public virtual void Update()
        {                
		UpdateState();
		 

	}

	protected virtual void FixedUpdate()
	{ 
		UpdateMovement();
	}

	void UpdateMovement()
	{
		bool movable = !isHit && (_state == MonsterState.Move || _state ==  MonsterState.Chasing) && _isObstacleAhead == false;
		  
		if (movable)
		{
			Vector3 vel = _rigid.velocity; 

			_rigid.velocity = new Vector3(transform.localScale.x * _moveSpeed, vel.y, vel.x); 
		}
	}
	
	string curAnim = "";
        public void UpdateState(string name = "") 
        {
		if (isHit)
			return;

		switch (_state)
		{
			case MonsterState.Chasing:
				PlayAnimation("Walk");
				OnChasing();
				break;
			case MonsterState.Waiting:
				PlayAnimation("Idle"); 
				OnWaiting();
				break;
			case MonsterState.Move:
				PlayAnimation("Walk");
				OnMove();
				break;
			case MonsterState.Attack: 
				OnAttack();
				break;
			case MonsterState.Death:
				PlayAnimation("Death");
				OnDeath(); 
				break;
			default:
				break;
		}
	}

	public void PlayAnimation(string nextAnim)
	{		 
		if (nextAnim != curAnim) 
		{
			curAnim = nextAnim;
		}
		_animator.Play(curAnim);
	}

	public virtual void OnChasing()
	{
		// 플레이어를 바라보며 달리기
		bool isLookingAtPlayer = (_player.transform.position.x - transform.position.x) * transform.localScale.x > 0;
		if (!isLookingAtPlayer)
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale =	scale; 
		}

		// 장애물에 걸렸다면
		if (_isObstacleAhead || isPlayerDead)
		{
			 _state = MonsterState.Waiting;
		}
		 
		// 공격 범위 까지 왔다면
		if (!isPlayerDead && _isPlayerInAttackRange)
		{
			_state = MonsterState.Attack;
		} 
	} 

	public virtual void OnWaiting()
        {
		_lastwaitTime += Time.deltaTime; 
		if (_lastwaitTime < _waitingTime)
			return;
		_lastwaitTime = 0.0f;
		 
		bool isLookingAtPlayer = (_player.transform.position.x - transform.position.x) * transform.localScale.x > 0;
		float distanceToPlayer = (_player.transform.position - transform.position).magnitude;
		if (!isPlayerDead && isLookingAtPlayer && distanceToPlayer < _playerTrackableRange)
			_state = MonsterState.Chasing;
		else
		{
			// 장애물에 막혀있다면 -1, 그렇지 않다면 랜덤하게 방향 설정
			int nextDir = _isObstacleAhead ? -1 : (UnityEngine.Random.Range(0, 2) * 2 - 1);  
			Vector3 scale = transform.localScale;
			scale.x *= nextDir;
			transform.localScale = scale;

			_state = MonsterState.Move; 
		}
	}



	public virtual void OnMove()
        {
		// 플레이어 추적 가능한 상태가 되면 추적 시작
		bool isLookingAtPlayer = (_player.transform.position.x - transform.position.x) * transform.localScale.x > 0;
		float distanceToPlayer = (_player.transform.position - transform.position).magnitude;
		if (!isPlayerDead && isLookingAtPlayer && distanceToPlayer < _playerTrackableRange)
		{
			_state = MonsterState.Chasing;
			_lastMoveTime = 0.0f;
			return;
		}

		// 일정시간 동안 움직인 이후 Waiting 상태로 변환
		_lastMoveTime += Time.deltaTime;
		if (_lastMoveTime < _moveTime)
			return;
		_lastMoveTime = 0.0f; 

		 
		_state = MonsterState.Waiting;
	}

	public abstract void OnAttack();
	Coroutine endHit = null;
	

	float lastHitTime = 0.0f;
	[SerializeField] float HitDelay = 1.0f;

        public virtual void OnHit(GameObject attacker, int damage, bool isBlocked = false)
        {
		if (_hp <= 0)
			return;

		_hp -= damage;

		_damageUI.SetDamage(damage); 
		if (_hpSlider != null) 
			_hpSlider.value = (float)_hp / _initHp;

		_effect?.PlayAnim("Hit", true); 
		if (Time.time - lastHitTime > HitDelay || isBlocked || _hp == 0)
		{
			PlayAnimation("Hit"); 
			lastHitTime = Time.time; 
		}
		  
		isHit = true;
		if (endHit != null)
			StopCoroutine(endHit);
		

		endHit = StartCoroutine(RegisterCoolTime(() => 
		{
			isHit = false;
			if (_hp <= 0)
			{

				DropItem();
				_state = MonsterState.Death;
				_onDead?.Invoke();
			}
			else
			{
				_state = MonsterState.Chasing;
			}
		})); 	

		
	}

	void DropItem()
	{
		foreach (DropItem item in _dropItems)
		{
			if (UnityEngine.Random.Range(0, 100) >= item.dropRate)
				continue;
			
			ItemSO itemSO = Instantiate<ItemSO>(item.item);
			ItemManager.instance.GetItemObj(itemSO, gameObject.transform.position);
		} 

	}

	public IEnumerator RegisterCoolTime(Action act)
	{
		yield return new WaitForEndOfFrame();
		float animLength = _animator.GetCurrentAnimatorStateInfo(0).length;
		
		yield return new WaitForSeconds(animLength);
		act.Invoke();
	} 

        void AE_EndHit()
        {
		
	}
        void AE_EndDeath()
        {
		_animator.speed = 0.0f; _hpUI.SetActive(false);
		Utils.instance.SetTimer(() => { _onDestroy?.Invoke(); }, 1.5f); 
	}

        void AE_OnAttack()
        {
                if (_isPlayerInAttackRange == false)
                        return;
                 
                 _player.GetComponent<PlayerActionController>()?.OnHit(gameObject);  

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
		//_lastAttackTime = 0.0f;
		//_animator.Play("Idle");
	}


        public abstract void OnDeath();

}
