using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
public class QuestDialogueSO : ScriptableObject
{
	public Texture2D npcIcon;
	public QuestSO quest;

	public DialogueSO pendingDialogue;   // ����Ʈ ���� �� DialogueSO
	public DialogueSO rejectedDialogue;  // ����Ʈ ���� DialogueSO
	public DialogueSO progressDialogue;  // ����Ʈ ���� �� DialogueSO
	public DialogueSO awaitingDialogue;  // ����Ʈ �Ϸ� ��� DialogueSO
	public DialogueSO completedDialogue; // ����Ʈ �Ϸ� ���� DialogueSO

	[NonSerialized] public EDialogueState state = EDialogueState.PENDING_ACCEPTANCE;
	[NonSerialized] DialogueSO curDialogue;

	[NonSerialized] public int curPage = 0;

	private void OnEnable()
	{
		
	}

	public DialogueSO GetCurDialogue()
	{
		bool isClear = true;
		foreach(QuestTaskSO task in quest.tasks)
		{
			if (task.curCnt < task.targetCnt)
			{
				isClear = false;
				break;
			} 
		}

		curPage = 0;
		if (state == EDialogueState.IN_PROGRESS && isClear)
			state = EDialogueState.AWAITING_COMPLETION;
		
		switch (state)
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
	public bool UpdateState(int playerIdx)
	{
		int npcSize = curDialogue.npc.Count; 

		EResponseType rspState = curDialogue.npc[curPage].player[playerIdx].state; 
		

		switch (rspState)
		{
			case EResponseType.CONTINUE:
				break;

			case EResponseType.END:
				return true; 

			case EResponseType.GET_REWARD:
				string reward = "";
				if (quest.reward != null)
					reward = quest.reward.GetReward();

				UIHandler.instance._questUIManager.LoadSuccessUI(reward);
				state = EDialogueState.COMPLETED; 
				break;

			case EResponseType.RECEIVE_QUEST:
				state = EDialogueState.IN_PROGRESS;
				QuestManager.instance.AddQuest(quest);
				break;

			case EResponseType.REJECT:
				state = EDialogueState.REJECTED;
				return true;

		};

		return false;
	}
} 