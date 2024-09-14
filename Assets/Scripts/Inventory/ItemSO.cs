using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Item/new Item", order = 1)]
public class ItemSO : ScriptableObject
{
	[NonSerialized] public int level;

	public Texture2D _itemIcon;
	public string _name;

	[TextArea(3, 10)]
	public string _description;

	public List<SkillAttribute> _effects; 

	public void InitItem()
	{
		level = GenLevel();
		
		for (int i = 0; i < level - _effects.Count; i++)
		{
			EPlayerStatus type = (EPlayerStatus)UnityEngine.Random.Range(0, (int)EPlayerStatus.Count);
			SkillAttribute sa = new SkillAttribute(type);
			_effects.Add(sa);
		}

		if (_effects.Count > 0) 
			_description += "\n";

		foreach (SkillAttribute sa in _effects)
		{
			sa.InitSkillAttribute();
			_description += "\n" + sa.descript;
		}
	}

	int GenLevel()
	{
		int ret = 1, range = 9;
		while (range > 1 && UnityEngine.Random.Range(0, range--) >= 1)
			ret++; 

		return ret = Mathf.Max(ret, _effects.Count);
	}

	public void EquipItem()
	{
		PlayerStats pe = PlayerStats.instance;
		foreach (SkillAttribute sa in _effects)
		{
			pe.ApplyEffect(sa.effectType, sa.value);
		}

	
	}

	public void UnequipItem()
	{
		PlayerStats pe = PlayerStats.instance;
		foreach (SkillAttribute sa in _effects)
		{ 
			pe.CancelEffect(sa.effectType, sa.value);
		}
	}
}

[Serializable]
public class SkillAttribute
{
	public SkillAttribute() { }
	public SkillAttribute(EPlayerStatus type, int value = -1) {
		effectType = type;
		if (value != -1)
			this.value = value;
	} 
	[SerializeField] public EPlayerStatus effectType = EPlayerStatus.Damage;
	[Space(10)]

	[SerializeField] int _minValue;
	[SerializeField] int _maxValue; 

	[SerializeField] public int value;
	[NonSerialized] public string descript;
	  
	public virtual void InitSkillAttribute()
	{
		if (_minValue + _maxValue == 0)
		{
			AttributeSO attribute = PlayerStats.instance.GetAttribute(effectType);
			_minValue = attribute.minItemStatValue;
			_maxValue = attribute.maxItemStatValue;
		}  
		 
		// 弥绊 可记 = red
		
		// 亮篮 可记 = yellow
		// 扁夯 可记 = x	

		if (value == 0)
			value = UnityEngine.Random.Range(_minValue, _maxValue+1);

		descript =  PlayerStats.instance.GetDescription(effectType) + " + " + value.ToString();

		if (value == _maxValue)
		{
			descript = "<color=red>" + descript + "</color>";
		}
		else if (value >= (_minValue + _maxValue) / 2)
		{
			descript = "<color=yellow>" + descript + "</color>";
		}
		descript = "- " + descript;
	}
}
