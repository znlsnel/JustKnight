using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Item/new Item", order = 1)]
public class ItemSO : ScriptableObject
{
	[NonSerialized] public RectTransform _rectTransform;
	public Texture2D _itemIcon;
	public string _name;
	[TextArea(3, 10)]
	public string _description;
	[NonSerialized] public int level;

	public List<SkillAttribute> _effects; 

	public void InitItem()
	{
		int rand = UnityEngine.Random.Range(0, 101);
		level = rand > 95 ? 5 : rand > 90 ? 4 : rand > 60 ? 3 : rand > 30 ? 2 : 1;
		if (level == 5) 
		{
			rand = UnityEngine.Random.Range(0, 11);
			level += rand <= 5 ? 0 : rand - 5;
		}
		level = Mathf.Max(level, _effects.Count);

		int cnt = level - _effects.Count;
		 
		while (cnt-- > 0)
		{
			EPlayerEffects type = (EPlayerEffects)UnityEngine.Random.Range(0, (int)EPlayerEffects.Count);
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

	public void EquipItem()
	{
		PlayerEffectManager pe = PlayerEffectManager.instance;
		foreach (SkillAttribute sa in _effects)
		{
			pe.ApplyEffect(sa.effectType, sa.value);
		}

	
	}

	public void UnequipItem()
	{
		PlayerEffectManager pe = PlayerEffectManager.instance;
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
	public SkillAttribute(EPlayerEffects type) {
		effectType = type;
	}
	[SerializeField] public EPlayerEffects effectType = EPlayerEffects.None;
	[Space(10)]

	[SerializeField] int _minValue;
	[SerializeField] int _maxValue; 

	[SerializeField] public int value;
	[NonSerialized] public string descript;
	  
	public virtual void InitSkillAttribute()
	{
		if (_minValue + _maxValue == 0)
		{
			Range range = PlayerEffectManager.instance.GetRange(effectType);
			_minValue = range._min;
			_maxValue = range._max;
		}  
		 
		// 弥绊 可记 = red
		
		// 亮篮 可记 = yellow
		// 扁夯 可记 = x	

		if (value == 0)
			value = UnityEngine.Random.Range(_minValue, _maxValue+1);

		descript =  PlayerEffectManager.instance.GetDescription(effectType) + " + " + value.ToString();

		if (value == _maxValue)
		{
			descript = "<color=red>" + descript + "</color>";
		}
		else if (value > (_minValue + _maxValue) / 2)
		{
			descript = "<color=yellow>" + descript + "</color>";
		}
		descript = "- " + descript;
	}
}
