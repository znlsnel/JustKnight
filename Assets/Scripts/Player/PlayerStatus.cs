using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerStatus : Singleton<PlayerStatus>
{
	private Dictionary<EPlayerStatus, AttributeSO> _attributes = new Dictionary<EPlayerStatus, AttributeSO>();
	 
	public int GetValue(EPlayerStatus type)
	{
		return _attributes[type].value + _attributes[type].addValue; 
	}

	public override  void Awake()
	{
		base.Awake();

		AttributeSO[] attributes = Resources.LoadAll<AttributeSO>("datas/attributes");

		foreach (AttributeSO attribute in attributes)
		{
			_attributes.Add(attribute.type, attribute); 
		}

	}

	public string GetDescription(EPlayerStatus type)
	{

		return _attributes[type].title; 
	}
	public AttributeSO GetAttribute(EPlayerStatus type)
	{
		return _attributes[type]; 
	}

	public void ApplyEffect(EPlayerStatus type, int value)
	{

		AttributeSO at;
		if (_attributes.TryGetValue(type, out at))
			at.addValue += value;
	}

	public void CancelEffect(EPlayerStatus type, int value)
	{

		AttributeSO at;
		if (_attributes.TryGetValue(type, out at))
			at.addValue -= value;

	}

	public string GetStatus()
	{
		string ret = "";
		for (EPlayerStatus type = (EPlayerStatus)0; type < EPlayerStatus.Count; type++)
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
