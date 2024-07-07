using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class QuestManager : Singleton<QuestManager> 
{
	public List<QuestSO> _quests = new List<QuestSO>();

	// Start is called before the first frame update
	public override void Awake()
	{
		base.Awake();
	} 

	void Start()
	{  

	} 

    // Update is called once per frame
	void Update()
	{
         
	}

	public QuestSO GetQuest(CategorySO category, TargetSO target)
	{
		foreach(QuestSO s in _quests)
		{
			if (s.task.category == category && s.task.target == target)
				return s;
		}
		return null;
	}

	public void InsertQuest(string npc_id)
	{
		//GameObject gm = Instantiate<GameObject>(_questInfoBtnPrefab);
	//	ButtonClickHandler b = gm.AddComponent<ButtonClickHandler>();
		
	//	b.RegisterButtonAction(() => { OnButtonDown(quest.id); }, () => { OnButtonUp(quest.id); });
		//gm.transform.SetParent(_questListUI.transform, false);
	//	gm.gameObject.GetComponentInChildren<Text>().text = quest.title;

	}


	public void OnButtonDown(string id)
	{

	} 
	 
	public void OnButtonUp(string id)
	{


	}

	public void KillMonster(GameObject monster)
	{

	}
}
