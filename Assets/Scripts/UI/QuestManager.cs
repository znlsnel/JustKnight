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
	List<Quest> _questInfoBtns =  new List<Quest>();
	  
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
		

		foreach (Quest quest in quests.quests)
		{ 
			InsertQuest(quest);
		} 
	}
	 
	public void InsertQuest(Quest quest)
	{
		GameObject gm = Instantiate<GameObject>(_questInfoBtnPrefab);
		ButtonClickHandler b = gm.AddComponent<ButtonClickHandler>();
		
		int idx = _questInfoBtns.Count;  
		b.RegisterButtonAction(() => { OnButtonDown(idx); }, () => { OnButtonUp(idx); });
		gm.transform.SetParent(_questListUI.transform, false);
		gm.gameObject.GetComponentInChildren<Text>().text = quest.title; 
		
		_questInfoBtns.Add(quest);  
	}


	public void OnButtonDown(int idx)
	{
	} 
	 
	public void OnButtonUp(int idx)
	{
		_questUIManager.UpdateQuestInfo(_questInfoBtns[idx]); 
		 
	}
}
