using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;

 
public class DialogueManager : MonoBehaviour , IMenuUI
{
	string _ncpName;
	private Dictionary<string, QuestDialogueSO> _dialogues = new Dictionary<string, QuestDialogueSO>();

	[SerializeField] Image _npcImage;
	[SerializeField] Text _npcScript;
	[SerializeField] Text[] _respScripts;

	QuestDialogueSO _curQuestDlg;
	DialogueSO _curDlg;

	private void Awake()  
	{
		gameObject.SetActive(false);
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
		  if (dialogue.npcIcon != null)
			_npcImage.sprite = Sprite.Create(dialogue.npcIcon, new Rect(0, 0, dialogue.npcIcon.width, dialogue.npcIcon.height), new Vector2(0.5f, 0.5f));
		_curQuestDlg = dialogue;
		_curDlg = _curQuestDlg.GetCurDialogue();

		UpdateDialougeText();
	}
	 
	private void UpdateDialougeText()
	{
		int page = _curQuestDlg.curPage;

		_npcScript.text = _curDlg.npc[page].text;
		int cnt = _curDlg.npc[page].player.Count;

		for (int i = 0; i < 3; i++) 
		{
			if (i < cnt)
			{ 
				_respScripts[i].transform.parent.gameObject.SetActive(true);
				_respScripts[i].text = _curDlg.npc[page].player[i].text; 
			}
			else
			{
				_respScripts[i].transform.parent.gameObject.SetActive(false);
			}
		}


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

		InputManager.instance.FreezeCharacter(active);

		gameObject.SetActive(active);
		UIHandler.instance.CloseAllUI(gameObject, active);
		if (!active)
			InputManager.instance._interactionHandler.Cancel(); 
	}
}
