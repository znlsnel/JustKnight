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

public class DialogueManager : MonoBehaviour , IMenuUI
{
	string _ncpName;
	private Dictionary<string, EpisodeSO> _dialogues = new Dictionary<string, EpisodeSO>();

	[SerializeField] TextMeshProUGUI _nameText;
	[SerializeField] TextMeshProUGUI _npcScript;
	[Space(10)]

	[SerializeField] GameObject _responseSlotPrefab;
	[SerializeField] GameObject _responseParent;

	EpisodeSO _curQuestDlg;
	DialogueSO _curDlg;

	Coroutine _updateScript;
	UnityEvent _completeScript = new UnityEvent();

	Action _onResponButton;

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
		if (_dialogues.ContainsKey(dialogue.name))
			return;

		_dialogues.Add(dialogue.name, dialogue);  
	}

	public void UpdateQuestDialogue(ref EpisodeSO dialogue)
	{
		if (!_dialogues.ContainsKey(dialogue.name))
			return;
		 
		dialogue = _dialogues[dialogue.name]; 
	}

	public bool RegisteEpisodes(List<EpisodeSO> episodes)
	{
		ActiveMenu(true);

		_npcScript.text = "";

		bool flag = false; 

		for (int i = 0; i < episodes.Count; i++)
		{ 
			int idx = i; 

			EpisodeSO ep = episodes[idx]; 
			UpdateQuestDialogue(ref ep); 

			if (ep.GetCurDialogue(false) == null || (ep.preQuest != null && !ep.preQuest.isClear)) 
				continue;
			 
			flag = true;

			if (episodes.Count == 1)
				BeginEpisode(episodes[0]);
			else
			{
				GameObject _responseSlot = Instantiate<GameObject>(_responseSlotPrefab);
				_responseSlot.GetComponent<DialogueResponseSlot>().InitResponseText(_responseParent, ep.episodeName, () => BeginEpisode(ep));
				_onResponButton += () => { Destroy(_responseSlot); };
			}
		} 

		return flag;
	}

	void BeginEpisode(EpisodeSO dialogue)
	{
		_onResponButton?.Invoke();
		_onResponButton = null;

		_curQuestDlg = dialogue;
		_curDlg = dialogue.GetCurDialogue(); 
		_nameText.text = dialogue.npcName;
		UpdateDialougeText();
	}
	 

	private void UpdateDialougeText()
	{
		int page = _curQuestDlg.curPage;

		_npcScript.text = "";
		if (_curDlg.npc[page] != null ) 
			_updateScript = StartCoroutine(UpdateScript(_curDlg.npc[page].text));
		 
		_completeScript?.RemoveAllListeners();
		_completeScript.AddListener(() =>  
		{
			_npcScript.text = _curDlg.npc[page].text;
			int cnt = _curDlg.npc[page].player.Count;

			for (int i = 0; i < cnt; i++)
			{
				int idx = i;
				GameObject _responseSlot = Instantiate<GameObject>(_responseSlotPrefab);
				_responseSlot.GetComponent<DialogueResponseSlot>().InitResponseText(_responseParent, _curDlg.npc[page].player[i].text, ()=>OnResponseButton(idx));
				_onResponButton += () => { Destroy(_responseSlot); };
			}
		});
	}

	IEnumerator UpdateScript(string text, int idx = 0)
	{
		while (idx < text.Length)
		{
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

		if (_curQuestDlg.UpdateState(id) ||_curQuestDlg.isEndPage(_curDlg))
		{
			ActiveMenu(false);
			return; 
		}
		_curQuestDlg.curPage++;

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
}
