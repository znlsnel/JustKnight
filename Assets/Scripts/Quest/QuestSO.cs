using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum EQuestState
{
	AWAITING,
	IN_PROGRESS,
	COMPLETED
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "new Quest", order = 1)]
public class QuestSO : ScriptableObject
{
	public string codeName;
	public string displayName;

	[TextArea(3, 10)]
	public string description;

	public List<QuestTaskSO> tasks;

	public RewardSO reward;
	public bool isCancelable;
	public bool isSavable;
	public bool isAutoComplete;

	bool clear = false;

	[NonSerialized] public EQuestState questState = EQuestState.AWAITING;

	[NonSerialized] public Action _onClear;
	public bool isClear 
	{ 
		get 
		{
			if (clear == true)
				return true;
			 
			return clear = CheckClear();
		} 
	}
	 
	bool CheckClear()
	{
		foreach (QuestTaskSO task in tasks)
		{
			if (task.curCnt < task.targetCnt)
				return false; 
		}
		return true;
	}
}
