using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class DisplayQuestSlotManager : MonoBehaviour
{
	public Text _questTitle;
	public List<GameObject> _questTasks = new List<GameObject>();
	 
	public void SetQuestSlot(QuestSO quest)
	{
		_questTitle.text = quest.displayName;

		for (int i = 0; i < 3; i++)
		{
			if (quest.tasks.Count > i)
			{
				_questTasks[i].SetActive(true);
				_questTasks[i].GetComponent<DisplayQuestTaskManager>().SetQuestInfo(quest.tasks[i].description, quest.tasks[i].curCnt, quest.tasks[i].targetCnt);
			} 
			else 
			{
				_questTasks[i].SetActive(false);
			}

		}
	}
}

