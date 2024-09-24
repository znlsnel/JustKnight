using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class QuestTrackerSlot : MonoBehaviour
{
	public Text _questTitle;
	public List<GameObject> _questTasks = new List<GameObject>();
	 
	public void SetQuestSlot(QuestSO quest)
	{
		_questTitle.text = quest.questName;

		for (int i = 0; i < 3; i++)
		{
			if (quest.tasks.Count > i)
			{
				_questTasks[i].SetActive(true);
				_questTasks[i].GetComponent<QuestTrackerTask>().SetQuestInfo(quest.tasks[i].taskTitle, quest.tasks[i].curCnt, quest.tasks[i].targetCnt);
			} 
			else 
			{
				_questTasks[i].SetActive(false);
			}

		}
	}
}

