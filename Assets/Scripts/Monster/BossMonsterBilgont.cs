using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterBilgont : BossMonster
{
	[SerializeField] Collider2D _skillSensor;
	bool _isPlayerInSkillRange { get {return CheckSensor(_skillSensor, "Player"); } } 

	[SerializeField] float dashPower = 10.0f;
	int attackState = 1; 
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
		PlayAnimation($"Attack{attackState}");

		_lastAttackTime = 0.0f;

		StartCoroutine(RegisterCoolTime(() => { isAttack = false; }));
	}

	public override void OnHit(GameObject attacker, int damage)
	{
		base.OnHit(attacker, damage);

		//if (monsterHp < _initHp / 2)
		//{
		//	attackState = 2;
		//}
	}

	public void AE_Pattern1()
	{
		float dir = transform.localScale.x > 0 ? 1 : -1;
		Vector3 vel = _rigid.velocity;
		vel.x = dir * dashPower;

		_rigid.velocity = vel;
	} 

	public void Pattern2()
	{ 
		if (_isPlayerInSkillRange)
			_player.GetComponent<PlayerActionController>()?.OnHit(gameObject);
		
	}
}
