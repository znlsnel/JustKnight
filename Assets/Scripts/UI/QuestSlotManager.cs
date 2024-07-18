using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotManager : MonoBehaviour
{
	public Text _questTitle;
	public Button _questSlotButton;
	public Button _questDisplayButton;

	QuestSO _quest;
	bool isDisplayed = false;

	public void SetQuestSlot(QuestSO quest)
	{
		_quest = quest;
		_questTitle.text = quest.description;
		_questSlotButton.onClick.AddListener( ()=>UIHandler.instance._questUIManager.UpdateQuestInfo(quest));
		_questDisplayButton.onClick.AddListener(() => AddDisplayQuest());
	}   

	public void AddDisplayQuest()
	{
		if (isDisplayed)
		{
			UIHandler.instance._displayQuestManager.RemoveQuest(_quest);
				Debug.Log("Remove ");
				isDisplayed = false;

			
		}
		else
		{
			if (UIHandler.instance._displayQuestManager.AddQuest(_quest))
			{
				isDisplayed = true;
				Debug.Log("ADD ");
			}
		}
	}

}
