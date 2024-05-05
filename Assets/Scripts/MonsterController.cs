using System;
using System.Collections;
using System.Collections.Generic;
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
        bool _isAttackable = true; 
        bool _onFrontCollisionSensor = false;
        bool _isInPlayerAttackRange = false;
         
        float _tracingIdleTime = 3.0f;
        float _tracingMoveTime = 2.0f;
        float _attackCoolTime = 3.0f;  
         
        float _lastIdleTime = 0.0f;
        float _lastMoveTime = 0.0f; 
        float _lastAttackTime = 0.0f;


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
                                _onFrontCollisionSensor = true;
                                _isInPlayerAttackRange = colliders[i].gameObject == _player; 
				break;
			}
                }
	}


    // Update is called once per frame 
    void Update()
    {
                UpdateSensor();

		_lastAttackTime += Time.deltaTime;

		Vector3 MonsterToPlayer = _player.transform.position - transform.position; 
                float DistanceToPlayer = MonsterToPlayer.magnitude;
		
		if (DistanceToPlayer < 3)
                {
                        if (MonsterToPlayer.x * transform.localScale.x  > 0.0f)
                        {
                                _isChasing = true;

			}

		}
                else if (DistanceToPlayer > 5)
                {
                        _isChasing = false;
                }

                if (_isChasing)
                {
                        Vector3 t = transform.localScale;
                        float tx = Math.Abs(t.x);
                        t.x = MonsterToPlayer.x > 0.0f ? tx : -tx;
                        transform.localScale = t;  
		}

                UpdateState();
    }

	private void FixedUpdate()
	{
		if (_state == MonsterState.Move)
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
		                _animator.Play("Attack1");
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

			if (_lastIdleTime > _tracingIdleTime)
                        {
                                _lastIdleTime = 0.0f;
                                _state = MonsterState.Move;

                                int nextDir = -1;

                                if (_onFrontCollisionSensor == false)
				        nextDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;  
                                 

				Vector3 ts = transform.localScale; 
				ts.x *= nextDir;
				transform.localScale = ts;  
			}

                        return;
                }

		if (_lastAttackTime < _attackCoolTime)
		{
			return;
		}

		_lastIdleTime = 0.0f;
		_state = MonsterState.Move;  
		
        }
        
        void OnMove() 
        {
                if (_isChasing == false)
                {
                        _lastMoveTime += Time.deltaTime;

                        if ( _lastMoveTime > _tracingMoveTime || _onFrontCollisionSensor)
                        {
                                _lastMoveTime = 0.0f; 
                                _state = MonsterState.Idle;
                        }
                        return;
                }

                if (_isInPlayerAttackRange)
		{
                        _state = MonsterState.Attack;
                } 


        }
         
        void OnAttack()
        {
                if (_lastAttackTime < _attackCoolTime)
                {
                        return;
		}

                _lastAttackTime = 0.0f;
                 

		if (_isInPlayerAttackRange == false)
                        _state = MonsterState.Idle;
	}

        void AE_EndAttack()
        {
		_state = MonsterState.Idle;

    }


	void OnDeath()
        {
                
        }

        void OnShield()
        {

        }
}
