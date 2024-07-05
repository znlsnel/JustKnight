using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster 
{
	
	public override void Start()
	{
		base.Start();
		_trackingDist = 20;

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


		_animator.Play("Attack");
	}

	public override void OnDeath()
	{
		
	}

}
