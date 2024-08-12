using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour, IMenuUI
{
        [SerializeField] TextMeshProUGUI _questTitleText;
        [SerializeField] TextMeshProUGUI _descriptionText;

	[Space(10)]
	[SerializeField] GameObject _questView_Pending;
	[SerializeField] GameObject _questView_InProgress;
	[SerializeField] GameObject _questView_completed;
	[SerializeField] GameObject _questView_displaying;

	[Space(10)]
	[SerializeField] GameObject _questParent_Pending;
	[SerializeField] GameObject _questParent_InProgress;
	[SerializeField] GameObject _questParent_completed;
	[SerializeField] GameObject _questParent_displaying;

	[Space(10)]
	[SerializeField] GameObject _questSlotPrefab;
	[SerializeField] GameObject _successUI;

	QuestSuccessUIManager _successUIManager;

	//Dictionary<QuestSO, QuestSlotManager> _slotManagers = new Dictionary<QuestSO, QuestSlotManager>();
	Dictionary<QuestSO, GameObject> _questObject = new Dictionary<QuestSO, GameObject>();
	private void Awake() 
	{
		_descriptionText.text = "";
		_successUI = Instantiate<GameObject>(_successUI);
		_successUIManager = _successUI.GetComponent<QuestSuccessUIManager>();
		
		DontDestroyOnLoad(_successUI); 

	}

	public void AddQuest(QuestSO quest)
	{
		GameObject gm = Instantiate<GameObject>(_questSlotPrefab);
		QuestSlotManager qsm = gm.GetComponent<QuestSlotManager>();
		gm.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		//_slotManagers.Add(quest, qsm); 
		if (_questObject.ContainsKey(quest) )
		{
			return; 
		}
		else
			_questObject.Add(quest, gm);


		qsm.SetQuestSlot(quest);
		gm.transform.SetParent(_questParent_Pending .transform);
	}


	// if you don't send anything as a factor, this function will empty the quest text
       public void UpdateQuestInfo(QuestSO quest = null)
	{
		
		_questTitleText.text = "";
		_descriptionText.text = "";

		if (quest == null)
			return;

		_questTitleText.text = quest.questName;
		_descriptionText.text += "<b>NPC : " + quest.npcName + "</b>\n\n";
		_descriptionText.text += quest.description + "\n\n";
		_descriptionText.text +="<color=white><b>";

		foreach (QuestTaskSO task in quest.tasks)
		{
			if (task.curCnt < task.targetCnt)
			{
				_descriptionText.text += "-" + task.description + "<color=yellow>( " + task.curCnt + " / " + task.targetCnt + " )</color>";
			}
			else
			{
				_descriptionText.text += task.description + "<color=yellow> [완료] </color>";
			}

			_descriptionText.text += "\n"; 
		}

	}

	public void MoveToCompletedQuests(QuestSO quest)
	{
		GameObject gm;
		if (_questObject.TryGetValue(quest, out gm))
			gm.transform.SetParent(_questParent_completed.transform);
	}

	public void ActiveMenu(bool active)
	{
		UIHandler.instance.CloseAllUI(gameObject, active);
		gameObject.SetActive(active);
		if (active)
		{
			UpdateQuestInfo(); 
		}
	}

	public void LoadSuccessUI(string rewardDescription)
	{
		_successUIManager.OpenSuccessUI(rewardDescription); 
	}

	public void OnListChangeButton(bool 진행중인퀘스트목록으로변경)
	{
		if (진행중인퀘스트목록으로변경)
		{
			_questView_InProgress.SetActive(true);
			_questView_completed.SetActive(false);
		}
		else
		{
			_questView_InProgress.SetActive(false);
			_questView_completed.SetActive(true);
		}
	}
}
