using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerStatus : Singleton<PlayerStatus>
{
	private Dictionary<EPlayerEffects, AttributeSO> _attributes = new Dictionary<EPlayerEffects, AttributeSO>();
	 
	public int GetValue(EPlayerEffects type)
	{
		return _attributes[type].value + _attributes[type].addValue; 
	}

	public override  void Awake()
	{
		base.Awake();

		string path = "datas/attributes";

		// Resources.LoadAll<T> 메서드를 사용하여 해당 경로에 있는 모든 AttributeSO를 불러옵니다.
		AttributeSO[] attributes = Resources.LoadAll<AttributeSO>(path);

		foreach (AttributeSO attribute in attributes)
		{
			_attributes.Add(attribute.type, attribute); 
		}
		// 불러온 attributes 배열을 반환합니다.

		//_descript.AddRange(new string[]
		//{
		//	"체력",
		//	"공격력", 
		//	"추가 공격 확률",
		//	"방어력",
		//	"막기 데미지",
		//	"회피 확률",
		//	"점프력",
		//	"이동 속도",
		//	"공격 속도",
		//	"부활 확률",
		//}) ;
	}

	public string GetDescription(EPlayerEffects type)
	{

		return _attributes[type].title; 
	}
	public AttributeSO GetAttribute(EPlayerEffects type)
	{

		return _attributes[type];
	}

	public void ApplyEffect(EPlayerEffects type, int value)
	{

		AttributeSO at;
		if (_attributes.TryGetValue(type, out at))
			at.addValue += value;
	}

	public void CancelEffect(EPlayerEffects type, int value)
	{

		AttributeSO at;
		if (_attributes.TryGetValue(type, out at))
			at.addValue -= value;

	}

	public string GetStatus()
	{
		string ret = "";
		for (EPlayerEffects type = (EPlayerEffects)0; type < EPlayerEffects.Count; type++)
		{
			ret += _attributes[type].title + " - " + (_attributes[type].value + _attributes[type].addValue).ToString();
			if (_attributes[type].addValue > 0)
			{
				ret += $"<color=yellow>(+{_attributes[type].addValue})</color>";
			}
			ret += "\n";
			
		}
		return ret;
	}
}
