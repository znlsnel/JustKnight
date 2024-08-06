using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "new ItemType", menuName = "Item/Effect", order = 1)]
public abstract class SkillAttributeSO : ScriptableObject
{
	int level;
	int value;

	public abstract void InitAttribute();
} 
