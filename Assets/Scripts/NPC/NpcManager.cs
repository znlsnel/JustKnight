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

			if (episode.preQuest == null || episode.preQuest.questState == EQuestState.COMPLETED)
				InitDialogue(episode);
			else
				episode.preQuest._onClear.Add(()=>InitDialogue(episode));
		}


		// EVENT라면 안보이게
		if (isEventNpc) GetComponent<SpriteRenderer>().sortingOrder = -1;
	}

	void InitDialogue(EpisodeSO episode)
	{
		QuestManager.instance.AddQuest(episode.quest);
		_dialogueManager.AddDialogue(episode);
		canStartQuest = true;

		// TODO State UI 표시
		if (_npcStateUI == null)
			return;

		_npcStateUI.SetNpcStateUI(episode);
		episode._onChangeState = () =>
		{ 
			if (_npcStateUI != null)
				_npcStateUI.SetNpcStateUI(episode);
		};
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
				_dialogues[i] = episode;

				if (_dialogues[i].GetDialogue(false) != null) 
				{
					flag = true;
					break;
				}
			} 

			if (!flag)
			{
				InputManager.instance._interactionHandler.Remove(gameObject);
				canStartQuest = false;
				yield break;
			}
			else
				yield return new WaitForSeconds(0.5f);
		}
	}


	//Collision을 체크하는 방식으로 변경 (현재는 오류가 있음 )
	IEnumerator StartConversation(GameObject target)
	{
		StartCoroutine(CheckQuestAvailability());
		while (canStartQuest == false || target.GetComponent<PlayerController>() == null)
			yield return new WaitForSeconds(0.5f);
		
		if (isAutoStart)
		{ 
			if (_dialogueManager.RegisteEpisodes(_dialogues))
				_onDialogue?.Invoke(); 
			yield break ;
		}

		InputManager.instance._interactionHandler.AddIAction(gameObject, () =>
		{
			if (_dialogueManager.RegisteEpisodes(_dialogues))
			{
				_onDialogue?.Invoke();

				InputManager.instance._interactionHandler.RegisterCancelAction(() => {
					_dialogueManager.ActiveMenu(false);
				});
			}  
			else
				InputManager.instance._interactionHandler.Remove(gameObject);
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
 