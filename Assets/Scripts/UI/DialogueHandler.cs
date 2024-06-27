using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Response
{
	public List<string> text;
	public int next;
}
 
[System.Serializable]
public class Dialogue
{
	public List<string> text;
	public List<Response> responses; 
	public string quest;
}

[System.Serializable]
public class Conversation
{
	public string quest; 
	public string npc;
	public List<Dialogue> dialogues;
}

public class DialogueHandler : MonoBehaviour 
{
	private Dictionary<string, Conversation> dialogues = new Dictionary<string, Conversation>();
	private Dictionary<string, int> loaded = new Dictionary<string, int>();
	Conversation curDialogue;

	string _ncpName;

	[SerializeField] Image _npcImage;
	[SerializeField] Text _npcScript;
	[SerializeField] Text[] _respScripts;

	int script_idx = 0;

	private void Awake()
	{ 
		gameObject.SetActive(false);
	}

	public void OpenNPCDialogue(string npc_id, int quest_id)
	{
		gameObject.SetActive(true); 
		curDialogue = GetDialogueData(npc_id, "quest_" + quest_id.ToString());
		 
		UpdateDialouge(); 
	}

	private void UpdateDialouge ()
	{
		_npcScript.text = "";
		foreach (string s in curDialogue.dialogues[script_idx].text)
			_npcScript.text += s + "\n";

		int cnt = curDialogue.dialogues[script_idx].responses.Count;
		for (int i = 0; i < 3; i++)
		{
			if (i < cnt)
			{
				_respScripts[i].transform.parent.gameObject.SetActive(true);
				_respScripts[i].text = curDialogue.dialogues[script_idx].responses[i].text[0];
			}
			else
			{
				_respScripts[i].transform.parent.gameObject.SetActive(false);
			}
		}
	}
	public void OnResponseButton(int id)
	{ 
		Debug.Log($"OnResponseButton : {id}");
		int next = curDialogue.dialogues[script_idx].responses[id].next;
		if (next < 0)
		{ 
			script_idx = -next;
			InputManager.instance._interactionHandler.ExcuteInteraction();
			return;
		}

		script_idx = next;  
		UpdateDialouge();

	} 


	public void CloseNPCDialogue()
	{
		gameObject.SetActive(false);
	}


	public Conversation GetDialogueData(string npc_id, string scriptId)
	{
		LoadDialogueData(npc_id);

		string key = npc_id + scriptId; 

		Conversation ret = null;
		dialogues.TryGetValue(key, out ret);

		return ret; 
	} 

	public void LoadDialogueData(string npc_id)
	{
		int temp = 0;
		if (loaded.TryGetValue(npc_id, out temp))
			return;

		loaded.Add(npc_id, temp); 
		
		
		string npcFilePath = Application.dataPath + "/Resources/Datas/dialogues/" + npc_id;

		string[] jsonFiles = Directory.GetFiles(npcFilePath, "*.json");
		string[] fileNames = new string[jsonFiles.Length];

		for (int i = 0; i < jsonFiles.Length; i++)
		{
			fileNames[i] = Path.GetFileName(jsonFiles[i]);
		}



		foreach (string json in fileNames) 
		{
			string filename = json.Split('.')[0];
			string key = npc_id + filename;
			//       npc_01 + quest_01

			 
			TextAsset jsonText = Resources.Load<TextAsset>("Datas/dialogues/" + npc_id +"/" + filename ); 
			 
			Conversation conversation = JsonUtility.FromJson<Conversation>(jsonText.text); 
			if (conversation != null)
				dialogues.Add(key, conversation);
		}
	}


}
