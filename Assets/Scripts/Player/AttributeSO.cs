using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EPlayerStatus : int
{
	None = -1,

	HP,
	Damage,
	DubbleAttackRate,
	Armor,
	ShieldDamage,
	AvoidanceRate,
	jumpPower,
	MoveSpeed,
	AttackSpeed,
	RevivalRate,

	Count,
}

[CreateAssetMenu(fileName = "new Attribute", menuName = "new Attribute", order = 1)]

public class AttributeSO : ScriptableObject
{
	public EPlayerStatus type;
	public int value;
	public string title;
	[TextArea(3, 10)]
	public string descript;
	
	[Space(10)]
	public int minItemStatValue;
	public int maxItemStatValue;

	[NonSerialized] public int addValue; 
}
