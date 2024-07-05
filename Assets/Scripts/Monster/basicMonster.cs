using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicMonster : Monster
{
	// Start is called before the first frame update
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
