using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/Category", order = 1)]
public class CategorySO : ScriptableObject
{
	public string codeName;
	public string displayName;
}
