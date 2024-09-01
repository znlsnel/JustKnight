using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[Serializable]
public class ReportInfo
{
	public CategorySO category;
	public TargetSO target;
}
public class QuestReporter : MonoBehaviour
{
	[SerializeField] public List<ReportInfo> reportInfos = new List<ReportInfo>();
	public int successCount = 1;
	public UnityEvent _enterCollider;

	DisplayQuest _displayQuest;
	QuestUI _questUI;

	private void Start()
	{
		_displayQuest = UIHandler.instance._displayQuest.GetComponent<DisplayQuest>();
		_questUI =  UIHandler.instance._mainMenu.GetComponent<MainMenu>()._questUI;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		_enterCollider?.Invoke();  
	}  

	public void Report(int idx)  
	{
		CategorySO category = reportInfos[idx].category;
		TargetSO target = reportInfos[idx].target; 

		List<Tuple<QuestSO,QuestTaskSO>> taskInfos = QuestManager.instance.GetQuest(category, target);
		 
		foreach(Tuple<QuestSO, QuestTaskSO> taskInfo in taskInfos)
		{
			QuestSO quest = taskInfo.Item1;
			QuestTaskSO task = taskInfo.Item2;

			if (quest._state != EQuestState.IN_PROGRESS)
				continue;

			task.curCnt = task.action.Run(task.curCnt, successCount);
			if (task.curCnt >= task.targetCnt)
			{
				if (quest.isClear)
				{
					if (quest.isAutoComplete)
						QuestManager.instance.CompleteQuest(quest, "");
					else
						quest._state = EQuestState.COMPLETED;
				} 
			}
		}
		if (taskInfos.Count > 0)
		{
			_displayQuest.UpdateDisplayQuestSlot();
			_questUI.UpdateQuestInfo();
		}

	}
}
