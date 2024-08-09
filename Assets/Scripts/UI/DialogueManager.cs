using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour , IMenuUI
{
	string _ncpName;
	private Dictionary<string, QuestDialogueSO> _dialogues = new Dictionary<string, QuestDialogueSO>();

	[SerializeField] Text _nameText;
	[SerializeField] Text _npcScript;
	[SerializeField] Text[] _respScripts;
	[Space(10)]

	[SerializeField] Color _pressColor;
	[SerializeField] Color _hoverColor;


	QuestDialogueSO _curQuestDlg;
	DialogueSO _curDlg;

	Coroutine _updateScript;
	UnityEvent _completeScript = new UnityEvent();
	
	private void Awake()   
	{ 
		gameObject.SetActive(false);

		foreach (Text _resp in _respScripts)
		{
			Color normal = _resp.color;
			ButtonHandler bch = _resp.gameObject.AddComponent<ButtonHandler>();
			{
				bch._onButtonDown = () => { _resp.color = _pressColor; };
				bch._onButtonUp = () => { _resp.color = normal; };
				bch._onButtonEnter = () => { _resp.color = _hoverColor; };
				bch._onButtonExit = () => { _resp.color = normal; };
			} 
		}
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
	public void AddDialogue(QuestDialogueSO dialogue)
	{
		if (_dialogues.ContainsKey(dialogue.name))
			return;

		_dialogues.Add(dialogue.name, dialogue);  
	}

	public QuestDialogueSO UpdateQuestDialogue(QuestDialogueSO dialogue)
	{
		return _dialogues[dialogue.name]; 
	}

	public void BeginDialogue(QuestDialogueSO dialogue)
	{
		ActiveMenu(true);
		_curQuestDlg = dialogue;
		_curDlg = _curQuestDlg.GetCurDialogue();
		_nameText.text = dialogue.npcName;
		UpdateDialougeText();
	}
	 

	private void UpdateDialougeText()
	{
		int page = _curQuestDlg.curPage;

		_npcScript.text = "";
		_updateScript = StartCoroutine(UpdateScript(_curDlg.npc[page].text));
		 
		_completeScript?.RemoveAllListeners();
		_completeScript.AddListener(() =>  
		{
			_npcScript.text = _curDlg.npc[page].text;
			int cnt = _curDlg.npc[page].player.Count;

			for (int i = 0; i < 3; i++)
			{
				if (i < cnt)
				{
					_respScripts[i].text = _curDlg.npc[page].player[i].text;
					_respScripts[i].transform.parent.gameObject.SetActive(true);
					//_respScripts[i].gameObject.GetComponent<ContentSizeFitter>()?.
				}
				else
				{
					_respScripts[i].transform.parent.gameObject.SetActive(false);
				}
			}
		});
		 
		for (int i = 0; i < 3; i++) 
		{
			_respScripts[i].transform.parent.gameObject.SetActive(false);
		}
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


	public void OnResponseButton(int id)
	{
		if (_curQuestDlg.UpdateState(id))
			ActiveMenu(false);

		_curQuestDlg.curPage++;  
		 
		if (_curDlg.npc.Count > _curQuestDlg.curPage) 
			UpdateDialougeText();
		else
			ActiveMenu(false);
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
