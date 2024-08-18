using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum EDialogueState
{
	PENDING_ACCEPTANCE,
	REJECTED,
	IN_PROGRESS,
	AWAITING_COMPLETION,
	COMPLETED
}

[CreateAssetMenu(fileName = "new Episode", menuName = "Dialogue/new Episode", order = 2)]
public class EpisodeSO : ScriptableObject
{
	public string npcName = "";
	public string episodeName = "";
	public QuestSO quest;
	public QuestSO preQuest;

	public DialogueSO pendingDialogue;   // ����Ʈ ���� �� DialogueSO
	public DialogueSO rejectedDialogue;  // ����Ʈ ���� DialogueSO
	public DialogueSO progressDialogue;  // ����Ʈ ���� �� DialogueSO
	public DialogueSO awaitingDialogue;  // ����Ʈ �Ϸ� ��� DialogueSO
	public DialogueSO completedDialogue; // ����Ʈ �Ϸ� ���� DialogueSO
	public Action _onChangeState;

	[NonSerialized] EDialogueState state = EDialogueState.PENDING_ACCEPTANCE;
	public EDialogueState _state
	{ 
		get {return state; } 
		set 
		{ 
			if (state != value)
			{
				state = value; 
				_onChangeState?.Invoke();
			}
		}
	}

	public bool isEndPage(DialogueSO dialogue)
	{
		return dialogue.npc.Count - 1 <= curPage; 
	}

	[NonSerialized] DialogueSO curDialogue;

	[NonSerialized] public int curPage = 0;

	private void OnEnable()
	{
		
	}
	public DialogueSO GetDialogue(bool init = true)
	{
		bool isClear = quest != null ? quest.isClear : false;
		  
		curPage = init ? 0 : curPage; 

		if (_state == EDialogueState.IN_PROGRESS && (quest == null || quest.isClear))
			_state = EDialogueState.AWAITING_COMPLETION;
		
		switch (_state)
		{
			case EDialogueState.PENDING_ACCEPTANCE: 
				curDialogue = pendingDialogue; // ���� �����
				break;

			case EDialogueState.REJECTED:
				curDialogue = rejectedDialogue; // �������� ���� �����
				break;

			case EDialogueState.IN_PROGRESS:
				curDialogue =  progressDialogue; // �� ���͸� ��ƾ���!
				break;

			case EDialogueState.AWAITING_COMPLETION: 
				curDialogue = awaitingDialogue; // ���� �����̾�!
				break;

			case EDialogueState.COMPLETED:
				curDialogue =  completedDialogue; // �������� ������!
				break;

			default:
				return null;
		}

		return curDialogue;
	} 

	//It returns true when the dialogue is finished,
	public bool UpdateState(DialogueSO dialogue, int playerIdx)
	{
		EResponseType rspState = dialogue.npc[curPage].player[playerIdx].state; 
		
		 
		switch (rspState)
		{
			case EResponseType.CONTINUE:
				break;

			case EResponseType.END:
				return true; 

			case EResponseType.GET_REWARD:
				{
					string reward = quest.reward != null ? quest.reward.GetReward() : "";
					QuestManager.instance.CompleteQuest(quest, reward);
					_state = EDialogueState.COMPLETED; 
				}
				break;

			case EResponseType.RECEIVE_QUEST:
				_state = EDialogueState.IN_PROGRESS;
				if (quest != null)
					QuestManager.instance.RegisterQuest(quest);
				break; 

			case EResponseType.REJECT:
				_state = EDialogueState.REJECTED;
				return true;

		};

		return false;
	}
} 