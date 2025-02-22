using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Security.Cryptography;
using UnityEngine.Pool;
using System;
using UnityEngine.UIElements;
using TMPro;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour, IMenuUI
{
	string _ncpName;
	Dictionary<string, EpisodeSO> _episodes = new Dictionary<string, EpisodeSO>();
	public Dictionary<string, EpisodeSO> episodes { get { return _episodes; } }


	[SerializeField] TextMeshProUGUI _nameText;
	[SerializeField] TextMeshProUGUI _npcScript;
	[Space(10)]

	[SerializeField] GameObject _responseSlotPrefab;
	[SerializeField] GameObject _responseParent;

	EpisodeSO _episode;
	DialogueSO _dialogue;

	Coroutine _updateScript;
	UnityEvent _completeScript = new UnityEvent();

	Action _onResponButton;
	Action _onEndDialogue;

	private void Awake()   
	{ 
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (_updateScript != null)
		{
			if (InputManager.instance.GetInputAction("Skip").IsPressed())
			{
				StopCoroutine(_updateScript);
				_completeScript?.Invoke();
				_completeScript?.RemoveAllListeners();
				_updateScript = null;  
			}
		}
	}
	public void AddDialogue(EpisodeSO dialogue)
	{
		if (_episodes.ContainsKey(dialogue.episodeCode))
			return;

		_episodes.Add(dialogue.episodeCode, dialogue);  
	}

	public void UpdateQuestDialogue(ref EpisodeSO dialogue)
	{
		if (!_episodes.ContainsKey(dialogue.episodeCode))
		{
			dialogue._state = EEpisodeState.PENDING_ACCEPTANCE;
			return;
		}
		 
		dialogue = _episodes[dialogue.episodeCode]; 
	}

	public EpisodeSO GetCurEpisode()
	{
		return _episode;
	}
	public bool RegisteEpisodes(List<EpisodeSO> episodes, UnityEvent action = null)
	{
		ActiveMenu(true);

		_npcScript.text = "";

		List<int> startableIdxs = new List<int>();
		for (int i = 0; i < episodes.Count; i++)
		{ 
			int idx = i; 

			EpisodeSO ep = episodes[idx]; 
			UpdateQuestDialogue(ref ep); 

			if (ep.GetDialogue(false) == null || (ep.preQuest != null &&  ep.preQuest._state != EQuestState.ENDED) )
				continue;
			  
			startableIdxs.Add(idx);
		} 
		
		if (startableIdxs.Count == 1)
			BeginEpisode(episodes[startableIdxs[0]]);
		else 
		{
			foreach (int i in startableIdxs) 
			{  
				int idx = i; 
				EpisodeSO ep = episodes[idx];
				GameObject _responseSlot = Instantiate<GameObject>(_responseSlotPrefab);
				_responseSlot.GetComponent<DialogueResponseSlot>().InitResponseText(_responseParent, ep.episodeName, () => BeginEpisode(ep));
				_onResponButton += () => { Destroy(_responseSlot); };
			}
		}
		_onEndDialogue = () => { action?.Invoke(); }; 

		return startableIdxs.Count > 0;
	}

	void BeginEpisode(EpisodeSO dialogue)
	{
		_onResponButton?.Invoke();
		_onResponButton = null;

		_episode = dialogue;
		_dialogue = dialogue.GetDialogue(); 
		_nameText.text = dialogue.npcName;
		UpdateDialougeText();
	}
	 

	private void UpdateDialougeText()
	{
		int page = _episode.curPage;

		_npcScript.text = "";
		if (_dialogue.npc[page] != null ) 
			_updateScript = StartCoroutine(UpdateScript(_dialogue.npc[page].text));
		 
		_completeScript?.RemoveAllListeners();
		_completeScript.AddListener(() =>  
		{
			_npcScript.text = _dialogue.npc[page].text;
			int cnt = _dialogue.npc[page].player.Count;

			for (int i = 0; i < cnt; i++)
			{
				int idx = i;
				GameObject _responseSlot = Instantiate<GameObject>(_responseSlotPrefab);
				_responseSlot.GetComponent<DialogueResponseSlot>().InitResponseText(_responseParent, _dialogue.npc[page].player[i].text, ()=>OnResponseButton(idx));
				_onResponButton += () => { Destroy(_responseSlot); };
			}
		});
	}

	IEnumerator UpdateScript(string text, int idx = 0)
	{
		while (idx < text.Length)
		{
			if (text[idx] == ' ')
				_npcScript.text += text[idx++];

			if (text[idx] == '<')
				while (text[idx++] != '>');
			
			_npcScript.text += text[idx++];
			yield return new WaitForSeconds(0.1f);
		}
		
		_completeScript?.Invoke();
		_completeScript?.RemoveAllListeners();
		
	}


	void OnResponseButton(int id)
	{
		_onResponButton.Invoke();
		_onResponButton = null;

		if (_episode.UpdateState(_dialogue, id) ||_episode.isEndPage(_dialogue))
		{
			ActiveMenu(false); 
			_onEndDialogue?.Invoke();
			_onEndDialogue = null;
			return; 
		}
		_episode.curPage++;

		UpdateDialougeText();
	}

	public void ActiveMenu(bool active)
	{
		if (active == gameObject.activeSelf)
			return;

		InputManager.instance.Freezz(active);
		 
		gameObject.SetActive(active);
		UIHandler.instance.CloseAllUI(gameObject, active);
		if (!active)
			InputManager.instance._interactionHandler.Cancel();

	}

	public void ResetEpisode()
	{
		_episodes.Clear();

		_nameText.text = "";
		_npcScript.text = "";

		 _episode = null;
		_dialogue = null;

		 if (_updateScript != null)
			StopCoroutine( _updateScript );
	}
}
