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

		// Resources.LoadAll<T> �޼��带 ����Ͽ� �ش� ��ο� �ִ� ��� AttributeSO�� �ҷ��ɴϴ�.
		AttributeSO[] attributes = Resources.LoadAll<AttributeSO>(path);

		foreach (AttributeSO attribute in attributes)
		{
			_attributes.Add(attribute.type, attribute); 
		}
		// �ҷ��� attributes �迭�� ��ȯ�մϴ�.

		//_descript.AddRange(new string[]
		//{
		//	"ü��",
		//	"���ݷ�", 
		//	"�߰� ���� Ȯ��",
		//	"����",
		//	"���� ������",
		//	"ȸ�� Ȯ��",
		//	"������",
		//	"�̵� �ӵ�",
		//	"���� �ӵ�",
		//	"��Ȱ Ȯ��",
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
