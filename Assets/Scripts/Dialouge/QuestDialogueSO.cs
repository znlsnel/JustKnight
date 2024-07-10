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
	public Image npcIcon;
	public QuestSO quest;

	public DialogueSO pendingDialogue;   // ����Ʈ ���� �� DialogueSO
	public DialogueSO rejectedDialogue;  // ����Ʈ ���� DialogueSO
	public DialogueSO progressDialogue;  // ����Ʈ ���� �� DialogueSO
	public DialogueSO awaitingDialogue;  // ����Ʈ �Ϸ� ��� DialogueSO
	public DialogueSO completedDialogue; // ����Ʈ �Ϸ� ���� DialogueSO
	 
	public EDialogueState state = EDialogueState.PENDING_ACCEPTANCE;

	[NonSerialized] public int curPage = 0;

	public DialogueSO GetCurDialogue(bool pageInit = true)
	{
		if (pageInit)
			curPage = 0;

		if (state == EDialogueState.IN_PROGRESS && quest.task.curCnt >= quest.task.targetCnt)
			state = EDialogueState.AWAITING_COMPLETION;
		
		switch (state)
		{
			case EDialogueState.PENDING_ACCEPTANCE:
				return pendingDialogue; // ���� �����

			case EDialogueState.REJECTED:
				return rejectedDialogue; // �������� ���� �����

			case EDialogueState.IN_PROGRESS:
				return progressDialogue; // �� ���͸� ��ƾ���!

			case EDialogueState.AWAITING_COMPLETION:
				return awaitingDialogue; // ���� �����̾�!

			case EDialogueState.COMPLETED:
				return completedDialogue; // �������� ������!

			default:
				return null;
		}
	}

	//It returns true when the dialogue is finished,
	public bool UpdateState(int plyerIdx)
	{
		EResponseType rspState = GetCurDialogue(false).npc[curPage].player[plyerIdx].state; 
		switch (rspState)
		{
			case EResponseType.CONTINUE:
				break;

			case EResponseType.END:
				return true; 

			case EResponseType.GET_REWARD:
				quest.reward.Get();
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