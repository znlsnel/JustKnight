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
	QuestUI _questUI;
	DisplayQuest _displayQuest;
	private void Start()
	{
		_questUI = UIHandler.instance._questUI.GetComponent<QuestUI>();
		_displayQuest = UIHandler.instance._displayQuest.GetComponent<DisplayQuest>();
	}

	public void SetQuestSlot(QuestSO quest)
	{
		_quest = quest;
		_questTitle.text = quest.description;
		_questSlotButton.onClick.AddListener( ()=> _questUI.UpdateQuestInfo(quest));
		_questDisplayButton.onClick.AddListener(() => AddDisplayQuest());
	}   

	public void AddDisplayQuest()
	{
		bool isDisplayed = _displayQuest.IsQuestSaved(_quest);

		if (isDisplayed) 
			_displayQuest.RemoveQuest(_quest);
		else
			_displayQuest.AddQuest(_quest);

		
	}

}
