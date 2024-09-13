using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewTask", menuName = "Quest/Task", order = 1)]
public class QuestTaskSO : ScriptableObject
{
	public CategorySO category;
	public TargetSO target;
	public TaskActionSO action;

	[Space(10)]
	public string taskTitle;
	public int targetCnt;

	[NonSerialized] public int curCnt = 0;
}
