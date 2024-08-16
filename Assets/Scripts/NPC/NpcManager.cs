using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NpcManager : MonoBehaviour
{
	[SerializeField] public List<EpisodeSO> _dialogues;
	[SerializeField] NpcStateUI _npcStateUI;
	[Space(10)]  public UnityEvent _onDialogue;
	[Space(10)]  public bool isAutoStart = false;
	[SerializeField] bool isEventNpc = false;

	QuestManager _questManager;
	DialogueManager _dialogueManager;
	QuestUI _questUI;

	Coroutine startConversation;

	bool canStartQuest = false;



	void Start()
        { 
		_questUI = _questUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._questUI;
		_dialogueManager = UIHandler.instance._dialogue.GetComponent<DialogueManager>();
		_questManager = QuestManager.instance;

		for (int i = 0; i < _dialogues.Count; i++)
		{
			EpisodeSO episode = _dialogues[i];
			_dialogueManager.UpdateQuestDialogue(ref episode);

			if (episode.preQuest == null || episode.preQuest.isClear)
				InitDialogue(episode);
			else
				episode.preQuest._onClear.Add(()=>InitDialogue(episode));
		}
		// 저장된 퀘스트 불러오기 


		


		// EVENT라면 안보이게
		if (isEventNpc) GetComponent<SpriteRenderer>().sortingOrder = -1;
	}

	void InitDialogue(EpisodeSO episode)
	{
		QuestManager.instance.AddQuest(episode.quest);
		_dialogueManager.AddDialogue(episode);
		_questUI.AddQuest(EQuestMenuType.PENDING, episode.quest);

		// TODO State UI 표시
		if (_npcStateUI != null)
		{
			_npcStateUI.SetNpcStateUI(episode);
			episode._onChangeState = () =>
			{
				if (_npcStateUI != null)
					_npcStateUI.SetNpcStateUI(episode);
			};
		}

		canStartQuest = true;
		
	}

	IEnumerator CheckQuestAvailability()
	{
		while (true)
		{
			bool flag = false;
			for(int i = 0; i < _dialogues.Count; i++ )
			{
				EpisodeSO episode = _dialogues[i];
				_dialogueManager.UpdateQuestDialogue(ref episode);
				if (episode.GetCurDialogue(false) != null)
				{
					flag = true;
					break;
				}
			} 

			if (!flag)
			{
				InputManager.instance._interactionHandler.Remove(gameObject);
				canStartQuest = false;
			}
			else
				yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator StartConversation(GameObject target)
	{
		while (canStartQuest == false || target.GetComponent<PlayerController>() == null)
			yield return new WaitForSeconds(0.5f);
		
		if (isAutoStart)
		{
			_dialogueManager.RegisteEpisodes(_dialogues);
			_onDialogue?.Invoke();
			yield break ;
		}

		StartCoroutine(CheckQuestAvailability());
		InputManager.instance._interactionHandler.AddIAction(gameObject, () =>
		{
			_dialogueManager.RegisteEpisodes(_dialogues);
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
	}
}
 