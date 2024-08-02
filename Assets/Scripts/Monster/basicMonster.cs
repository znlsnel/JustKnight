using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicMonster : Monster
{
	// Start is called before the first frame update
	public override void OnAttack()
	{
		if (_isPlayerInAttackRange == false)
		{
			_state = MonsterState.Chasing;
			return;
		}

		if (isAttack)
			return;
		 
		_lastAttackTime += Time.deltaTime;
		if (_lastAttackTime < _attackCoolTime)
		{
			PlayAnimation("Idle");
			return;
		}
		
		isAttack = true;
		PlayAnimation("Attack1");
		_lastAttackTime = 0.0f;

		StartCoroutine(RegisterCoolTime(() => { isAttack = false; })); 
	} 

	public override void OnDeath()
	{
		 
	}

}
