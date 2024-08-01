using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewTask", menuName = "Quest/Task", order = 1)]
public class QuestTaskSO : ScriptableObject
{
	public CategorySO category;
	public string displayName;

	[TextArea(3, 10)]
	public string description;

	public TaskActionSO action;
	public TargetSO target;
	 
	public int targetCnt;
	[NonSerialized] public int curCnt = 0;
}
