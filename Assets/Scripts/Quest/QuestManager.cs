using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

struct QuestInfo
{
	CategorySO category;
	TargetSO target;

	public QuestInfo(CategorySO c, TargetSO t)
	{
		category = c;
		target = t;	
	}
}

public class QuestManager : Singleton<QuestManager> 
{
	private Dictionary<QuestInfo, HashSet<QuestSO>> _quests = new Dictionary<QuestInfo, HashSet<QuestSO>>();

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
		
		Debug.Log($"frame : {1.0f / Time.deltaTime}");
	}

	public List<Tuple<QuestSO, QuestTaskSO>> GetQuest(CategorySO category, TargetSO target)
	{

		List<Tuple<QuestSO, QuestTaskSO>> ret = new List<Tuple<QuestSO, QuestTaskSO>>();

		HashSet<QuestSO> foundQuests;

		if (_quests.TryGetValue(new QuestInfo(category, target), out foundQuests))
		{
			foreach (QuestSO s in foundQuests)
			{
				foreach (QuestTaskSO task in s.tasks)
					if (task.category == category && task.target == target)
						ret.Add(new Tuple<QuestSO, QuestTaskSO>(s, task));
			}
		}
		Debug.Log($"GetQuest frame : {1.0f / Time.deltaTime}");
		return ret;
	}

	public void AddQuest(QuestSO quest)
	{
		UIHandler.instance._questUIManager.AddQuest(quest);

		foreach (QuestTaskSO task in quest.tasks)
		{
			HashSet<QuestSO> myQuests;
			if (_quests.TryGetValue(new QuestInfo(task.category, task.target), out myQuests))
				myQuests.Add(quest);
			else
			{ 
				myQuests = new HashSet<QuestSO>();
				myQuests.Add(quest);

				_quests.Add(new QuestInfo(task.category, task.target), myQuests); 
			}
		}
		Debug.Log($"AddQuest frame : {1.0f / Time.deltaTime}");

	}

	public void RemoveQuest(QuestSO quest)
	{
		foreach (QuestTaskSO task in quest.tasks)
		{
			HashSet<QuestSO> foundQuests;

			if (_quests.TryGetValue(new QuestInfo(task.category, task.target), out foundQuests))
			{
				if (foundQuests.Contains(quest))
				{
					foundQuests.Remove(quest);
					if (foundQuests.Count == 0)
						_quests.Remove(new QuestInfo(task.category, task.target));
				}
			}
		}
		Debug.Log($"RemoveQuest frame : {1.0f / Time.deltaTime}");

	}

}
