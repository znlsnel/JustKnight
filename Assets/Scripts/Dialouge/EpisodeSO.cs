using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum EEpisodeState
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

	[NonSerialized] EEpisodeState state = EEpisodeState.PENDING_ACCEPTANCE;
	public string episodeCode { get { return npcName + episodeName;} }

	public EEpisodeState _state
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

		if (_state == EEpisodeState.IN_PROGRESS && (quest != null && quest.isClear))
			_state = EEpisodeState.AWAITING_COMPLETION;
		
		switch (_state)
		{
			case EEpisodeState.PENDING_ACCEPTANCE: 
				curDialogue = pendingDialogue; // ���� �����
				break;

			case EEpisodeState.REJECTED:
				curDialogue = rejectedDialogue; // �������� ���� �����
				break;

			case EEpisodeState.IN_PROGRESS:
				curDialogue =  progressDialogue; // �� ���͸� ��ƾ���!
				break;

			case EEpisodeState.AWAITING_COMPLETION: 
				curDialogue = awaitingDialogue; // ���� �����̾�!
				break;

			case EEpisodeState.COMPLETED:
				curDialogue =  completedDialogue; // �������� ������!
				break;

			default:
				return null;
		}

		return curDialogue;
	}

	// It returns true when the dialogue has finished 
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
					_state = EEpisodeState.COMPLETED; 
				}
				break;

			case EResponseType.RECEIVE_QUEST:
				_state = EEpisodeState.IN_PROGRESS;
				if (quest != null)
				{
					quest._state = EQuestState.IN_PROGRESS;
					QuestManager.instance.RegisterQuest(quest, true);
				}
				break; 

			case EResponseType.REJECT:
				_state = EEpisodeState.REJECTED;
				return true;

		};

		return false;
	}
} 