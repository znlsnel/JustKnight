using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Response
{
	public string id;
	public List<string> text;
	public string nextDialogueId;
}

[System.Serializable]
public class Dialogue
{
	public string id;
	public List<string> text;
	public List<Response> responses;
}

[System.Serializable]
public class Conversation
{
	public string id; 
	public string npc;
	public List<Dialogue> dialogues;
}

public class DialogueHandler : MonoBehaviour 
{
	private Dictionary<string, Conversation> dialogues = new Dictionary<string, Conversation>();
	private Dictionary<string, int> loaded = new Dictionary<string, int>();
	Conversation curDialogue;

	string _ncpName;

	Canvas _canvas;

	[SerializeField] Image _npcImage;
	[SerializeField] Text _npcScript;

	[SerializeField] Text _respScript_1;
	[SerializeField] Text _respScript_2;
	[SerializeField] Text _respScript_3;

	private void Awake()
	{
		_canvas = GetComponent<Canvas>();
	}

	public void OpenNPCDialogue(string npc_id, int quest_id)
	{

		gameObject.SetActive(true);

		//LoadDialogueData(npc_id);
		//GetDialogueData(npc_id, quest_id.ToString()); 
	}

	public void CloseNPCDialogue()
	{
		gameObject.SetActive(false);
	}


	public void GetDialogueData(string npc_id, string scriptId)
	{
		string key = npc_id + "/" + scriptId;

		Conversation ret = null;
		dialogues.TryGetValue(key, out ret);

		if (ret == null)
		{
			ret = JsonUtility.FromJson<Conversation>(key);
			if (ret != null)
				dialogues.Add(key, ret);
		}

		curDialogue = ret;
	} 

	public void LoadDialogueData(string npc_id)
	{
		int temp = 0;
		if (loaded.TryGetValue(npc_id, out temp))
			return;
		else
		{ 
			loaded.Add(npc_id, temp); 
		}

		string npcFilePath = "dialogues/" + npc_id;

		string[] jsonFiles = Directory.GetFiles(npcFilePath, "*.json");

		foreach (string json in jsonFiles) 
		{
			string key = npc_id + json;

			Conversation conversation = JsonUtility.FromJson<Conversation>(key);
			if (conversation != null)
				dialogues.Add(key, conversation);
		}
	}


}
