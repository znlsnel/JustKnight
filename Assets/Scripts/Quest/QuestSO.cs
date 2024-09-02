using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum EQuestState
{
	PENDING,
	IN_PROGRESS,
	COMPLETED,
	ENDED
}
 
[CreateAssetMenu(fileName = "NewQuest", menuName = "new Quest", order = 1)]
public class QuestSO : ScriptableObject
{
	public string npcName;
	public string questName;
	[TextArea(3, 10)]
	public string description;

	public List<QuestTaskSO> tasks;

	public RewardSO reward;
	public bool isCancelable;
	public bool isSavable;
	public bool isAutoComplete;

	[NonSerialized] private bool clear = false;

	[NonSerialized] EQuestState questState = EQuestState.PENDING;
	public EQuestState _state 
	{
		get { return questState; }
		set
		{
			if (value == questState)
				return;

			questState = value;
			_onChangeState?.Invoke();
		}
	}

	public Action _onChangeState;


	[NonSerialized] public HashSet<Action> _onClear = new HashSet<Action>();
	public string questCode { get { return npcName + questName; } }

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
