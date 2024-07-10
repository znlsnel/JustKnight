using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;


public class DialogueHandler : MonoBehaviour , IMenuUI
{
	string _ncpName;

	[SerializeField] Image _npcImage;
	[SerializeField] Text _npcScript;
	[SerializeField] Text[] _respScripts;

	QuestDialogueSO _curQuestDlg;
	DialogueSO _curDlg;


	private void Awake()  
	{
		gameObject.SetActive(false);
	}

	public void BeginDialogue(QuestDialogueSO dialogue)
	{
		ActiveMenu(true);

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


	public void CloseNPCDialogue()
	{
		ActiveMenu(false); 
	}

	public void OnResponseButton(int id)
	{
		if (_curQuestDlg.UpdateState(id))
			CloseNPCDialogue();

		_curQuestDlg.curPage++;  
		 
		if (_curDlg.npc.Count > _curQuestDlg.curPage) 
			UpdateDialougeText();
		else
			CloseNPCDialogue();
	}

	public void ActiveMenu(bool active)
	{
		gameObject.SetActive(active);
		UIHandler.instance.CloseAllUI(gameObject, active); 
	}
}
