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

	public DialogueSO pendingDialogue;   // 퀘스트 수락 전 DialogueSO
	public DialogueSO rejectedDialogue;  // 퀘스트 거절 DialogueSO
	public DialogueSO progressDialogue;  // 퀘스트 진행 중 DialogueSO
	public DialogueSO awaitingDialogue;  // 퀘스트 완료 대기 DialogueSO
	public DialogueSO completedDialogue; // 퀘스트 완료 이후 DialogueSO
	 
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
				return pendingDialogue; // 몬스터 잡아줘

			case EDialogueState.REJECTED:
				return rejectedDialogue; // 거절하지 말고 잡아줘

			case EDialogueState.IN_PROGRESS:
				return progressDialogue; // 저 몬스터를 잡아야해!

			case EDialogueState.AWAITING_COMPLETION:
				return awaitingDialogue; // 고마워 보상이야!

			case EDialogueState.COMPLETED:
				return completedDialogue; // 저번에는 고마웠어!

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