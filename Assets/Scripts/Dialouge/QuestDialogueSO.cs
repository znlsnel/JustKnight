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

	[NonSerialized] public EDialogueState state = EDialogueState.PENDING_ACCEPTANCE;
	[NonSerialized] DialogueSO curDialogue;

	[NonSerialized] public int curPage = 0;
	 
	public DialogueSO GetCurDialogue()
	{
		curPage = 0;
		if (state == EDialogueState.IN_PROGRESS && quest.task.curCnt >= quest.task.targetCnt)
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

		Debug.Log($"Player Text IDX : {playerIdx}");
		int npcSize = curDialogue.npc.Count; 
		Debug.Log($"Page Size : {npcSize} , current Page : {curPage}");  


		EResponseType rspState = curDialogue.npc[curPage].player[playerIdx].state; 
		

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