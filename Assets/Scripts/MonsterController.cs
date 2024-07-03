using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterController : MonoBehaviour 
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

	protected bool _isChasing = false;

	protected bool _onFrontCollisionSensor = false;
        protected bool _isInPlayerAttackRange = false;

	protected float _tracingIdleTime = 1.0f;
	protected float _tracingMoveTime = 2.0f;
	protected float _attackCoolTime = 1.0f;

	protected float _lastIdleTime = 0.0f;
	protected float _lastMoveTime = 0.0f;
	protected float _lastAttackTime = 0.0f;
         
        public int hp = 3;

        public abstract void InitMonster(Vector3 pos); 
          
     
        public virtual void Start()
        { 
                _animator = GetComponent<Animator>();
                _player = GameObject.FindWithTag("Player");
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
                                         _onFrontCollisionSensor = colliders[i].GetComponent<MonsterController>() == null;

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
		
                if (DistanceToPlayer < 5 && MonsterToPlayer.x * transform.localScale.x > 0.0f)
                        _isChasing = true;
		 
                if (DistanceToPlayer > 10 || _player.GetComponent<PlayerAnimCtrl>()?.state == PlayerAnimCtrl.PlayerState.Death)
                        _isChasing = false;


                float dist = Math.Abs(_player.transform.position.x - transform.position.x);
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

	public abstract  void UpdateState();


	public abstract void OnIdle();
	public abstract void OnMove();
        public abstract void OnAttack();
          
        public void OnHit(GameObject attacker)
        {
                if (hp == 0)
                        return;

                hp--; 
		_state = MonsterState.Hit;
                _isChasing = true;
	} 

        void AE_EndHit()
        {
                _lastAttackTime = _attackCoolTime;
                 
                if (hp == 0)
                        _state = MonsterState.Death;
                else
                        _state = MonsterState.Idle;
                 
	}
        void AE_EndDeath()
        {
                _animator.speed = 0.0f;
        }

        void AE_OnAttack()
        {
                if (_isInPlayerAttackRange == false)
                        return;
                 
                 _player.GetComponent<PlayerController>()?.OnHit(gameObject); 

        } 

        void AE_EndAttack()
        { 
		_lastAttackTime = 0.0f;
		_animator.Play("Idle");
	}


        public abstract void OnDeath();

}
