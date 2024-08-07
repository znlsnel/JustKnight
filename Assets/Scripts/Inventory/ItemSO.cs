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

	public void Init()
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

		foreach (SkillAttribute sa in _effects)
		{
			sa.InitSkillAttribute();
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
	[SerializeField] EPlayerEffects effectType = EPlayerEffects.None;
	[Space(10)]

	[SerializeField] int _minValue;
	[SerializeField] int _maxValue; 

	[SerializeField] public int value;
	  
	public virtual void InitSkillAttribute()
	{
		if (_minValue + _maxValue == 0)
		{
			Range range = PlayerEffectManager.instance.ApplyAbility(effectType, 0);
			_minValue = range._min;
			_maxValue = range._max;
		}
		 
		if (value == 0)
			value = UnityEngine.Random.Range(_minValue, _maxValue+1);  
		 
		PlayerEffectManager.instance.ApplyAbility(effectType, value);
	}
}
