using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class monsterQuestInfo
{
	public GameObject monster; 
	public int targetCnt;
	[NonSerialized] public int cnt;
}

public class HuntMonsterQuestStep : QuestStep
{
	[SerializeField] List<monsterQuestInfo> _quests;
	[SerializeField] Dictionary<string, monsterQuestInfo> _questMap;

	int completeCount = 0;
	private void Awake()
	{
		foreach (monsterQuestInfo quest in _quests)
			_questMap.Add(quest.monster.name, quest);
		
	}

	//private int itemCount = 0;
	public void SetItemCount(GameObject monster)
	{
		monsterQuestInfo quest;
		if (_questMap.TryGetValue(monster.name, out quest))
		{ 
			quest.cnt++;

			if (quest.cnt == quest.targetCnt)
				completeCount++;
		}

		if (completeCount == _quests.Count)
		{
			FinishQestStep();
		}
	}
}
