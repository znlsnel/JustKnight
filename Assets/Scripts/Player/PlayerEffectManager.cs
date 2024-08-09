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

public struct  Attribute
{
	public int init;
	public int min;
	public int max;

	public Attribute(int init, int min, int max)
	{
		this.init = init;
		this.min = min;
		this.max = max;
	}
}
public class PlayerEffectManager : Singleton<PlayerEffectManager>
{
	[NonSerialized] public List<Attribute> _ranges = new List<Attribute>();
	[NonSerialized] public List<int> _effects = new List<int>((int)EPlayerEffects.Count);
	[NonSerialized] public List<string> _descript = new List<string>((int)EPlayerEffects.Count);

	public Attribute _hp					= new Attribute(3, 1, 3);
	public Attribute _power				= new Attribute(1, 1, 3);
	public Attribute _dubbleAttackRate		= new Attribute(0, 2, 6);
	public Attribute _armor				= new Attribute(0, 1, 3);
	public Attribute _reflectDamage		= new Attribute(0, 1, 3);
	public Attribute _avoidanceRate		= new Attribute(0, 1, 5);
	public Attribute _jumpPower			= new Attribute(100, 1, 3);
	public Attribute _moveSpeed			= new Attribute(100, 1, 3);
	public Attribute _attackSpeed			= new Attribute(100, 1, 3);
	public Attribute _revivalRate			= new Attribute(0, 1, 3); 

	public override  void Awake()
	{
		base.Awake();

		for (int i =0; i < (int)EPlayerEffects.Count; i++)
		{
			_effects.Add(0); 
		}

		_ranges.AddRange(new Attribute[] 
		{
			_hp, 
			_power, 
			_dubbleAttackRate, 
										
			_armor, 
			_reflectDamage,
			_avoidanceRate, 
										
			_jumpPower, 
			_moveSpeed,  
			_attackSpeed,
										
			_revivalRate   
		});

		_descript.AddRange(new string[]
		{
			"체력",
			"공격력", 
			"추가 공격 확률",
			"방어력",
			"반사 데미지",
			"회피 확률",
			"점프력",
			"이동 속도",
			"공격 속도",
			"부활 확률",
		}) ;
	}

	public string GetDescription(EPlayerEffects type)
	{
		if (type == EPlayerEffects.None || type == EPlayerEffects.Count)
			return "";

		return _descript[(int)type]; 
	}
	public Attribute GetRange(EPlayerEffects type)
	{
		if (type == EPlayerEffects.None || type == EPlayerEffects.Count)
			return new Attribute();

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

	public string GetStatus()
	{
		string ret = "";
		for (int i = 0; i <(int)EPlayerEffects.Count; i++)
		{
			ret += _descript[i] + " - " + (_ranges[i].init + _effects[i]).ToString();
			if (_effects[i] > 0)
			{
				ret += $"<color=yellow>(+{_effects[i]})</color>";
			}
			ret += "\n";
			
		}
		return ret;
	}
}
