using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReporter : MonoBehaviour
{
	[SerializeField] public CategorySO category;
	[SerializeField] public TargetSO target;

	public int successCount = 0;

	public void Report()
	{ 
		QuestSO quest = QuestManager.instance.GetQuest(category, target);
		if (quest != null )
		{
			quest.task.curCnt = quest.task.action.Run(quest.task.curCnt, successCount);
			//if (quest.task.curCnt >= quest.task.targetCnt)
			//{ 
			//	quest.reward.Get();
			//}
		}
	}
}
