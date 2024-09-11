using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicMonster : Monster
{
	// Start is called before the first frame update


	public override void OnAttack()
	{
		if (isAttack)
			return;
		
		if (isPlayerDead || _isPlayerInAttackRange == false)
		{
			_state = MonsterState.Chasing;
			return;
		}
		 
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

	public override void InitMonster(Vector3 pos)
	{
		base.InitMonster(pos);
		_hpSlider.gameObject.transform.position = _hpPos.position; 

	}

	public override void Update()
	{
		base.Update();
		_hpSlider.gameObject.transform.position = _hpPos.position;
	}

}
