using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

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
	private Dictionary<QuestInfo, HashSet<QuestSO>> _tasks = new Dictionary<QuestInfo, HashSet<QuestSO>>();
	public Dictionary<string, QuestSO> _quests = new Dictionary<string, QuestSO>(); 

	public List<Tuple<QuestSO, QuestTaskSO>> GetQuest(CategorySO category, TargetSO target)
	{
		List<Tuple<QuestSO, QuestTaskSO>> ret = new List<Tuple<QuestSO, QuestTaskSO>>();
		HashSet<QuestSO> foundQuests; 

		if (_tasks.TryGetValue(new QuestInfo(category, target), out foundQuests))
		{

			var tasks = foundQuests
				.AsParallel()
				.SelectMany(quest => quest.tasks
					.Where(task => task.category == category && task.target == target)
						.Select(task => new Tuple<QuestSO, QuestTaskSO>(quest, task)))
				.ToList();
			ret.AddRange(tasks);

		}
		return ret; 
	}

	public void AddQuest(QuestSO quest)
	{
		if (_quests.ContainsKey(quest.codeName))
			return;

		_quests.Add(quest.codeName, quest);
		foreach (QuestTaskSO task in quest.tasks)
		{
			HashSet<QuestSO> myQuests;
			if (_tasks.TryGetValue(new QuestInfo(task.category, task.target), out myQuests))
				myQuests.Add(quest);
			else
			{
				myQuests = new HashSet<QuestSO> { quest };

				_tasks.Add(new QuestInfo(task.category, task.target), myQuests);
			}
		}
	}

	public void RegisterQuest(QuestSO quest)
	{
		quest = UpdateQuestData(quest); 
		UIHandler.instance._questUIManager.AddQuest(quest);
		UIHandler.instance._displayQuestManager.AddQuest(quest);
		quest.questState = EQuestState.IN_PROGRESS;
	}

	public void RemoveQuestTasks(QuestSO quest)
	{
		foreach (QuestTaskSO task in quest.tasks)
		{
			HashSet<QuestSO> foundQuests;

			if (_tasks.TryGetValue(new QuestInfo(task.category, task.target), out foundQuests))
			{
				if (foundQuests.Contains(quest))
				{
					foundQuests.Remove(quest);
					if (foundQuests.Count == 0)
						_tasks.Remove(new QuestInfo(task.category, task.target));
				}
			}
		}
		Debug.Log($"RemoveQuest frame : {1.0f / Time.deltaTime}");
	}


	public void CompleteQuest(QuestSO quest, string rewardInfo)
	{
		UIHandler.instance._questUIManager.LoadSuccessUI(rewardInfo);
		UIHandler.instance._questUIManager.MoveToCompletedQuests(quest);

		QuestManager.instance.RemoveQuestTasks(quest);
		quest.questState = EQuestState.COMPLETED;
	}

	public QuestSO UpdateQuestData(QuestSO quest)
	{
		return _quests[quest.codeName]; 
	}
}
