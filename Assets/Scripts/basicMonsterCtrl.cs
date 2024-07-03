using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicMonsterCtrl : MonsterController
{
	// Start is called before the first frame update
	public override void Start()
	{
		base.Start();
	} 

    // Update is called once per frame
	public override void Update()
	{
		base.Update();

	}

	public override void InitMonster(Vector3 pos)
	{
		gameObject.SetActive(true);
		hp = 3;
		_state = MonsterState.Idle; 
		gameObject.transform.position = pos;
	}

	public override void UpdateState() 
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
				OnDeath();
				_animator.Play("Death");
				break;
			default:
				break;
		}
	}
	 
	public override void OnIdle()
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

	public override void OnMove()
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

	public override void OnAttack()
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

	public override void OnDeath()
	{
		
	}

}
