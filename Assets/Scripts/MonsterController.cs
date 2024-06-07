using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
        // Start is called before the first frame update

        Animator _animator;
        GameObject _player;
        Rigidbody2D _rigid;
        Collider2D _frontCollisionSensor;

        enum MonsterState
        { 
                Idle,
                Move, 
                Attack,
                Death,
                Hit,
                Shield
        }
	MonsterState _state = MonsterState.Idle;
         
        [SerializeField] float _moveSpeed = 1.0f;

        bool _isChasing = false;

        bool _onFrontCollisionSensor = false;
        bool _isInPlayerAttackRange = false;
         
        float _tracingIdleTime = 1.0f;
        float _tracingMoveTime = 2.0f;
        float _attackCoolTime = 1.0f;  
         
        float _lastIdleTime = 0.0f;
        float _lastMoveTime = 0.0f; 
        float _lastAttackTime = 0.0f;

        public int hp = 3;

        public void InitMonster() 
        {
                hp = 3;
                _state = MonsterState.Idle; 
        }

        void Start()
        { 
                _animator = GetComponent<Animator>();
                _player = GameObject.FindWithTag("Player");
		_rigid = GetComponent<Rigidbody2D>();
		_frontCollisionSensor = transform.Find("CollisionSensor_Front").GetComponent<Collider2D>(); 
	}

        const float SensorUpdateCycle = 1.0f / 30.0f;
        float lastSensorUpdateTime = 0.0f;
        void UpdateSensor()
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
                                if (_onFrontCollisionSensor == false)
                                         _onFrontCollisionSensor = colliders[i].GetComponent<MonsterController>() == null;

                                if (_isInPlayerAttackRange == false && colliders[i].gameObject == _player)
                                {
                                        _isInPlayerAttackRange = colliders[i].gameObject.GetComponent<PlayerAnimCtrl>()?.state != PlayerAnimCtrl.PlayerState.Death;
				} 
                                  
			}
                }
	}
        

    void Update()
    {
                UpdateSensor(); 
                 

		Vector3 MonsterToPlayer = _player.transform.position - transform.position; 
                float DistanceToPlayer = MonsterToPlayer.magnitude;
		
		if (DistanceToPlayer < 5 && MonsterToPlayer.x * transform.localScale.x > 0.0f)
                        _isChasing = true;
		 
                if (DistanceToPlayer > 10 || _player.GetComponent<PlayerAnimCtrl>()?.state == PlayerAnimCtrl.PlayerState.Death)
                        _isChasing = false;
                 
                 
                if (_isChasing && _state == MonsterState.Move) 
                {
                        if (Math.Abs(_player.transform.position.x - transform.position.x) > 0.1f) 
                        {
                                Vector3 t = transform.localScale;
                                float tx = Math.Abs(t.x);
                                t.x = MonsterToPlayer.x > 0.0f ? tx : -tx;
                                transform.localScale = t;  

                        }

		}

                UpdateState();
    }  

	private void FixedUpdate()
	{
                bool movable = _state == MonsterState.Move && _onFrontCollisionSensor == false;
                
		if (movable)
                {   
                        Vector2 nextPos = transform.position + new Vector3(transform.localScale.x * _moveSpeed, -1.0f, 0.0f) * Time.fixedDeltaTime;
			_rigid.MovePosition(nextPos);  
		}
	} 

	void UpdateState()
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
				_animator.Play("Hit" );
				break;
			case MonsterState.Death:
				OnDeath();
				_animator.Play("Death");
				break;
                        case MonsterState.Shield:
				OnShield();
				_animator.Play("Shield");
				break;
                                 
                        default:
                                break;
		}

        }



        void OnIdle()
        {
		if (_isChasing == false)
                { 
			_lastIdleTime += Time.deltaTime;
			if (_lastIdleTime < _tracingIdleTime)
				return; 

			_lastIdleTime = 0.0f;
                        _state = MonsterState.Move;

                        int nextDir = _onFrontCollisionSensor ? -1 : (UnityEngine.Random.Range(0, 2) * 2) - 1;                           
                          
			Vector3 ts = transform.localScale; 
			ts.x *= nextDir;
			transform.localScale = ts;     
                         
                        return;
                }


		bool isClosePlayer = Math.Abs(_player.transform.position.x - transform.position.x) < 0.1f;
		if (_isInPlayerAttackRange) 
		{
			_state = MonsterState.Attack;
		}
		else if (isClosePlayer == false)
                { 
		        _state = MonsterState.Move;  
                }
		
        }
        
        void OnMove() 
        {
		if (_isChasing == false)
		{
		        _lastMoveTime += Time.deltaTime;

			if (_lastMoveTime < _tracingMoveTime && _onFrontCollisionSensor == false)
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

	void OnAttack() 
        {
		_lastAttackTime += Time.deltaTime;
                if (_lastAttackTime < _attackCoolTime)
                {
		        return; 
		}

		if (_isInPlayerAttackRange == false)   
		{
			_state = MonsterState.Idle;
			return;
		}
                 
		_lastAttackTime = 0.0f;

                 
		_animator.Play("Attack1");  
	}
          
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


	void OnDeath()
        {
                
        }

        void OnShield()
        {

        }
}
