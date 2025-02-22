using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotManager : MonoBehaviour
{
	public TextMeshProUGUI _questTitle;
	public TextMeshProUGUI _checkBoxText; 
	public Button _questSlotButton;
	public Button _questDisplayButton;

	QuestSO _quest;
	QuestUI _questUI;
	QuestTracker _questTracker;
	private void Awake()
	{
		_questUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._questUI;
		_questTracker = UIHandler.instance._displayQuest.GetComponent<QuestTracker>(); 
	}

	public void SetQuestSlot(QuestSO quest, bool displaying = false)
	{
		_quest = quest;
		_questTitle.text = quest.questName;
		_questSlotButton.onClick.AddListener( ()=> _questUI.UpdateQuestInfo(quest));
		_questDisplayButton.onClick.AddListener(() => AddDisplayQuest());
		_checkBoxText.gameObject.SetActive(displaying);


	}   

	public void AddDisplayQuest(bool destory = false)
	{
		bool isDisplayed = _questTracker.IsQuestStored(_quest);

		if (destory || isDisplayed)
		{
			_questTracker.RemoveQuest(_quest);
			_checkBoxText.gameObject.SetActive(false);
			_questUI.DisplayingQuestCheck(_quest, false);
		}
		else
		{
			_questTracker.AddQuest(_quest);
			_checkBoxText.gameObject.SetActive(true);
		} 
	}

}
