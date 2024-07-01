using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

#region json
[Serializable]
public class MonsterHunt
{
	public string monsterName;
	public int huntCount;  
}
 
[Serializable]
public class Delivery
{
	public string npcName;
	public string itemName; 
	public int itemCount;
}
[Serializable]
public class Reward
{
	public  int gold;
	public string itemName;
	public int count;
}

[System.Serializable]
public class Quest
{
	public string id;
	public string preQuest;
	public string title;
	public string description;
	public MonsterHunt monsterHunt = null;
	public Delivery delivery = null;
	public Reward reward = null;
}

[Serializable]
public class Quests
{
	public List<Quest> quests;
}
#endregion


public class QuestManager : MonoBehaviour 
{
	Dictionary<string , Quests> _quests = new Dictionary<string , Quests>();
	Dictionary<string, Quest> _myQuests = new Dictionary<string, Quest> ();
	  
	[SerializeField] GameObject _questInfoBtnPrefab;  
	[SerializeField] GameObject _questListUI;

	QuestUIManager _questUIManager;

	// Start is called before the first frame update
	private void Awake()
	{

	}
	void Start()
	{  
		LoadDialogueData("npc_01");
		gameObject.SetActive(false);
		_questUIManager = gameObject.GetComponent<QuestUIManager>(); 
	} 

    // Update is called once per frame
	void Update()
	{
         
	}

	public void LoadDialogueData(string npc_id)
	{
		Quests temp;  
		if (_quests.TryGetValue(npc_id, out temp))
			return;

                string path = "Datas/dialogues/" + npc_id + "/quests"; 
		TextAsset jsonText = Resources.Load<TextAsset>(path);
		Quests quests = JsonUtility.FromJson<Quests>(jsonText.text); 
		if (quests != null)
			_quests.Add(npc_id, quests); 
		

	} 
	  
	public void AddQuest(string npc_id, int quest_id)
	{
		
		string questId = "quest_0" + quest_id.ToString();
		 
		Debug.Log(questId);
		Quests quests;
		if (!_quests.TryGetValue(npc_id, out quests)) 
			return;

		Quest q;
		if (_myQuests.TryGetValue(questId, out q))
		{
			Debug.Log("¿ÃπÃ ¿÷¡ˆ∑’!");
			return;

		}

		foreach (Quest quest in quests.quests)
		{
			if (quest.id == questId) 
			{
				InsertQuest(quest);
				break;
			}
		}
	}

	public void InsertQuest(Quest quest)
	{
		GameObject gm = Instantiate<GameObject>(_questInfoBtnPrefab);
		ButtonClickHandler b = gm.AddComponent<ButtonClickHandler>();
		
		b.RegisterButtonAction(() => { OnButtonDown(quest.id); }, () => { OnButtonUp(quest.id); });
		gm.transform.SetParent(_questListUI.transform, false);
		gm.gameObject.GetComponentInChildren<Text>().text = quest.title;

		_myQuests.Add(quest.id, quest);  
	}


	public void OnButtonDown(string id)
	{
	} 
	 
	public void OnButtonUp(string id)
	{
		Quest q;
		_myQuests.TryGetValue(id, out q);
		 
		if (q != null)
			_questUIManager.UpdateQuestInfo(q); 
	}
}
