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

	[NonSerialized] public EQuestState questState = EQuestState.AWAITING;
}
