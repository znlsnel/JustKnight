using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	public int successCount = 0; 

	public void Report(int idx) 
	{
		CategorySO category = reportInfos[idx].category;
		TargetSO target = reportInfos[idx].target; 

		List<Tuple<QuestSO,QuestTaskSO>> taskInfos = QuestManager.instance.GetQuest(category, target);
		 
		foreach(Tuple<QuestSO, QuestTaskSO> taskInfo in taskInfos)
		{
			QuestSO quest = taskInfo.Item1;
			QuestTaskSO task = taskInfo.Item2;

			task.curCnt = task.action.Run(task.curCnt, successCount);
			if (quest.isAutoComplete && task.curCnt >= task.targetCnt)
			{
				bool isClear = true;
				foreach (QuestTaskSO t in quest.tasks)
				{
					if (t.curCnt < t.targetCnt)
					{
						isClear = false;
						break;
					}
				}
				if (isClear)
				{
					QuestManager.instance.RemoveQuest(quest);
					UIHandler.instance._questUIManager.LoadSuccessUI("");
				}
			}
		}
		if (taskInfos.Count > 0)
			UIHandler.instance._displayQuestManager.UpdateDisplayQuestSlot(); 

	}
}
