using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EPlayerEffects : int
{
	None = -1,
	AddHp,
	AddDamage,
	DubbleAttackRate,
	Armor,
	ReflectDamage,
	AvoidanceRate,
	jumpPower,
	MoveSpeed,
	AttackSpeed,
	TeleportSkillLevelUp,
	TeleportAddDistance,
	RevivalCnt,


	Count,
} 

public class PlayerEffectManager : Singleton<PlayerEffectManager>
{
	[NonSerialized] public int _addHp;
	[NonSerialized]  public int _addDamage;
	[NonSerialized] public int _dubbleAttackRate;
	[NonSerialized] public int _armor;
	[NonSerialized] public int _reflectDamage; 
	[NonSerialized] public int _avoidanceRate;
	[NonSerialized] public int _jumpPower;
	[NonSerialized] public int _moveSpeed;
	[NonSerialized] public int _attackSpeed;
	[NonSerialized] public int _teleportSkillLevelUp;
	[NonSerialized] public int _teleportAddDistance;
	[NonSerialized] public int _revivalCnt;

	public Tuple<int, int> _rangeAddHp;
	public Tuple<int, int> _rangeAddDamage; 
	public Tuple<int, int> _rangeDubbleAttackRate;
	public Tuple<int, int> _rangeArmor;
	public Tuple<int, int> _rangeReflectDamage;
	public Tuple<int, int> _rangeAvoidanceRate;
	public Tuple<int, int> _rangeJumpPower;
	public Tuple<int, int> _rangeMoveSpeed; 
	public Tuple<int, int> _rangeAttackSpeed;
	public Tuple<int, int> _rangeTeleportSkillLevelUp;
	public Tuple<int, int> _rangeTeleportAddDistance;
	public Tuple<int, int> _rangeRevivalCnt;


	public Tuple<int, int> ApplyAbility(EPlayerEffects type, int value)
	{
		switch (type)
		{
			case EPlayerEffects.None:
				break;
			case EPlayerEffects.AddHp:
				_addHp += value;
				return _rangeAddHp;

			case EPlayerEffects.AddDamage:
				_addDamage += value;
				return _rangeAddDamage;

			case EPlayerEffects.DubbleAttackRate:
				_dubbleAttackRate += value;
				return _rangeDubbleAttackRate;

			case EPlayerEffects.Armor:
				_armor += value;
				return _rangeArmor;

			case EPlayerEffects.ReflectDamage:
				_reflectDamage += value;
				return _rangeReflectDamage;

			case EPlayerEffects.AvoidanceRate:
				_avoidanceRate += value;
				return _rangeAvoidanceRate;

			case EPlayerEffects.jumpPower:
				_jumpPower += value;
				return _rangeJumpPower;

			case EPlayerEffects.MoveSpeed:
				_moveSpeed += value;
				return _rangeMoveSpeed;

			case EPlayerEffects.AttackSpeed:
				_attackSpeed += value;
				return _rangeAttackSpeed;

			case EPlayerEffects.TeleportSkillLevelUp:
				_teleportSkillLevelUp += value;
				return _rangeTeleportSkillLevelUp;

			case EPlayerEffects.TeleportAddDistance:
				_teleportAddDistance += value;
				return _rangeTeleportAddDistance;

			case EPlayerEffects.RevivalCnt:
				_revivalCnt += value;
				return _rangeRevivalCnt; 
		}

		return null;
	}

}
