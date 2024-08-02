using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster 
{
	public override void OnAttack()
	{
		_lastAttackTime += Time.deltaTime;
		if (_lastAttackTime < _attackCoolTime)
		{
			return;
		} 

		if (_isPlayerInAttackRange == false)
		{
			_state = MonsterState.Waiting; 
			return;
		}

		_lastAttackTime = 0.0f;
	}

	public override void OnDeath()
	{
		
	}

}
