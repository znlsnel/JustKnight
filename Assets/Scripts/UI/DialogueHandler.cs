using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;


public class DialogueHandler : MonoBehaviour 
{
	string _ncpName;

	[SerializeField] Image _npcImage;
	[SerializeField] Text _npcScript;
	[SerializeField] Text[] _respScripts;
	 
	DialogueSO _curDialogue;
	int script_idx = 0;

	private void Awake()  
	{
		gameObject.SetActive(false);
	}

	public void BeginDialogue(DialogueSO dialogue)
	{
		gameObject.SetActive(true);

		_curDialogue = dialogue;
		script_idx = 0;

		UpdateDialouge();
	}

	private void UpdateDialouge()
	{
		_npcScript.text = _curDialogue.npc[script_idx].text;

		int cnt = _curDialogue.npc[script_idx].player.Count;
		for (int i = 0; i < 3; i++) 
		{
			if (i < cnt)
			{
				_respScripts[i].transform.parent.gameObject.SetActive(true);
				_respScripts[i].text = _curDialogue.npc[script_idx].player[i].text; 
			}
			else
			{
				_respScripts[i].transform.parent.gameObject.SetActive(false);
			}
		}
		
		if (_curDialogue.npc[script_idx].quest != null)
		{
			QuestManager.instance.AddQuest(_curDialogue.npc[script_idx].quest);
		}
	}


	public void CloseNPCDialogue()
	{
		gameObject.SetActive(false);
	}

	public void OnResponseButton(int id)
	{
		if (_curDialogue.npc[script_idx].player[id].isEnd)
		{
			CloseNPCDialogue();
		}
		script_idx++;
		UpdateDialouge();
	}


}
