using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class QuestManager : Singleton<QuestManager> 
{
	private List<QuestSO> _quests = new List<QuestSO>();

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

	public List<Tuple<QuestSO, QuestTaskSO>> GetQuest(CategorySO category, TargetSO target)
	{
		List<Tuple<QuestSO, QuestTaskSO>> ret = new List<Tuple<QuestSO, QuestTaskSO>>();
		foreach(QuestSO s in _quests)
		{
			foreach (QuestTaskSO task in s.tasks)
				if (task.category == category && task.target == target) 
					ret.Add(new Tuple<QuestSO, QuestTaskSO>(s, task)); 
		}
		return ret;
	}

	public void AddQuest(QuestSO quest)
	{
		UIHandler.instance._questUIManager.AddQuest(quest);
		_quests.Add(quest); 
	}

	public void RemoveQuest(QuestSO quest)
	{
		_quests.Remove(quest);
	}

}
