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
	RevivalCnt,


	Count,
}

[Serializable]
public struct  Range
{
	public int _min;
	public int _max;
}
public class PlayerEffectManager : Singleton<PlayerEffectManager>
{
	[NonSerialized] public List<Range> _ranges = new List<Range>();
	[NonSerialized] public List<int> _effects = new List<int>((int)EPlayerEffects.Count);
	[NonSerialized] public List<string> _descript = new List<string>((int)EPlayerEffects.Count);

	public Range _rangeAddHp;
	public Range _rangeAddDamage; 
	public Range _rangeDubbleAttackRate;
	public Range _rangeArmor;
	public Range _rangeReflectDamage;
	public Range _rangeAvoidanceRate;
	public Range _rangeJumpPower;
	public Range _rangeMoveSpeed; 
	public Range _rangeAttackSpeed;
	public Range _rangeRevivalCnt;

	public override  void Awake()
	{
		base.Awake();

		for (int i =0; i < (int)EPlayerEffects.Count; i++)
		{
			_effects.Add(0); 
		}

		_ranges.AddRange(new Range[] 
		{
			_rangeAddHp, 
			_rangeAddDamage, 
			_rangeDubbleAttackRate, 
										
			_rangeArmor, 
			_rangeReflectDamage,
			_rangeAvoidanceRate, 
										
			_rangeJumpPower, 
			_rangeMoveSpeed,  
			_rangeAttackSpeed,
										
			_rangeRevivalCnt   
		});

		_descript.AddRange(new string[]
		{
			"체력",
			"추가 데미지", 
			"추가 공격 확률",
			"방어력",
			"반사 데미지",
			"회피 확률",
			"점프력",
			"이동 속도",
			"공격 속도",
			"부활 횟수",
		}) ;
	}

	public string GetDescription(EPlayerEffects type)
	{
		if (type == EPlayerEffects.None || type == EPlayerEffects.Count)
			return "";

		return _descript[(int)type]; 
	}
	public Range GetRange(EPlayerEffects type)
	{
		if (type == EPlayerEffects.None || type == EPlayerEffects.Count)
			return new Range();

		return _ranges[(int)type];
	}

	public void ApplyEffect(EPlayerEffects type, int value)
	{
		if (type == EPlayerEffects.None || type == EPlayerEffects.Count)
			return;

		_effects[(int)type] += value;  
	}

	public void CancelEffect(EPlayerEffects type, int value)
	{
		if (type == EPlayerEffects.None || type == EPlayerEffects.Count)
			return;

		_effects[(int)type] -= value; 
	}
}
