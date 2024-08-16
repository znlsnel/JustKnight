using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NpcManager : MonoBehaviour
{
	[SerializeField] public List<QuestDialogueSO> _dialogues;
	[SerializeField] NpcStateUI _npcStateUI;
	[Space(10)]

	QuestManager _questManager;
	DialogueManager _dialogueManager;
	QuestDialogueSO _curDialogue;
	QuestUI _questUI;

	public UnityEvent _onDialogue;

	[Space(10)]
        public bool isAutoStart = false;
	public bool isEventNpc = false;

	bool hasCompletedPreQuest = false;

	Coroutine startConversation;

	void Start()
        { 
		_questUI = _questUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._questUI;
		_dialogueManager = UIHandler.instance._dialogue.GetComponent<DialogueManager>();
		_questManager = QuestManager.instance;

		_curDialogue = _dialogues[0];

		// 저장된 퀘스트 불러오기
		QuestDialogueSO t = _dialogueManager.UpdateQuestDialogue(_curDialogue);
		if (t != null)
			_curDialogue = t;

		if (_curDialogue.preQuest == null || _curDialogue.preQuest.isClear)
			InitDialogue();
		else
			_curDialogue.preQuest._onClear.Add(InitDialogue);


		// EVENT라면 안보이게
		if (isEventNpc) GetComponent<SpriteRenderer>().sortingOrder = -1;
	}

	void InitDialogue()
	{
		Debug.Log("ADDQUESTINFO");
		QuestManager.instance.AddQuest(_curDialogue.quest);
		_dialogueManager.AddDialogue(_curDialogue);
		_questUI.AddQuest(EQuestMenuType.PENDING, _curDialogue.quest);

		// State UI 표시
		if (_npcStateUI != null)
		{
			_npcStateUI.SetNpcStateUI(_curDialogue);
			_curDialogue._onChangeState = () =>
			{
				if (_npcStateUI != null)
					_npcStateUI.SetNpcStateUI(_curDialogue);
			};
		}


		hasCompletedPreQuest = true;
	}

        // Update is called once per frame
        void Update()
        {
		
        }

	IEnumerator CheckQuestAvailability()
	{
		while (true)
		{
			_curDialogue = _dialogueManager.UpdateQuestDialogue(_curDialogue);
			if (_curDialogue.GetCurDialogue(false) == null)
			{
				InputManager.instance._interactionHandler.Remove(gameObject);
				yield break;
			}
			yield return new WaitForSeconds(0.5f); 

		}
	}
        //     
        // [ NPC 1] [ NPC 2] [ NPC 3] [ NPC 4] 

	IEnumerator StartConversation(GameObject target)
	{
		while (hasCompletedPreQuest == false || _curDialogue.GetCurDialogue() == null || target.GetComponent<PlayerController>() == null)
			yield return new WaitForSeconds(0.5f);
		
		if (isAutoStart)
		{
			_dialogueManager.BeginDialogue(_curDialogue);
			_onDialogue?.Invoke();
			yield break ;
		}

		StartCoroutine(CheckQuestAvailability());
		InputManager.instance._interactionHandler.AddIAction(gameObject, () =>
		{
			_curDialogue = _dialogueManager.UpdateQuestDialogue(_curDialogue);
			if (_curDialogue.GetCurDialogue(false) == null)
				return;

			_dialogueManager.BeginDialogue(_curDialogue);
			_onDialogue?.Invoke();

			InputManager.instance._interactionHandler.RegisterCancelAction(() => {
				_dialogueManager.ActiveMenu(false);
			});
		});
	}

	private void OnTriggerEnter2D(Collider2D collision)  
	{
		startConversation = StartCoroutine(StartConversation(collision.gameObject));
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>() == null)
			return;

		if (startConversation != null)
			StopCoroutine(startConversation);

		InputManager.instance._interactionHandler.Remove(gameObject);
		//Debug.Log("NPC 상호작용 목록에 등록 해제");

	}

}
