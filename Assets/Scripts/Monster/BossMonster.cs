using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster 
{


	public override void OnAttack()
	{
		if (isPlayerDead || _isPlayerInAttackRange == false)
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
		PlayAnimation($"Attack{curAttackAnim}");
		curAttackAnim = ((curAttackAnim + 1) % AttackCnt) + 1;

		_lastAttackTime = 0.0f;

		StartCoroutine(RegisterCoolTime(() => { isAttack = false; }));
	}

	public override void OnDeath()
	{
		
	}


	

}
