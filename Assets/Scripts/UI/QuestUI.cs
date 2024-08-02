using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour, IMenuUI
{
        [SerializeField] Text _descriptionText;

	[SerializeField] GameObject _activeQuestScrollList;
	[SerializeField] GameObject _completedQuestScrollList;
	[SerializeField] GameObject _activeQuestParent;
	[SerializeField] GameObject _completedQuestParent; 

	[SerializeField] GameObject _questObjPrefab;
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
		gameObject.SetActive(false); 
	}

	public void AddQuest(QuestSO quest)
	{
		GameObject gm = Instantiate<GameObject>(_questObjPrefab);
		QuestSlotManager qsm = gm.GetComponent<QuestSlotManager>();

		//_slotManagers.Add(quest, qsm); 
		if (_questObject.ContainsKey(quest) )
		{
			return; 
		}
		else
			_questObject.Add(quest, gm);


		qsm.SetQuestSlot(quest);
		gm.transform.SetParent(_activeQuestParent.transform);
	}


	// if you don't send anything as a factor, this function will empty the quest text
       public void UpdateQuestInfo(QuestSO quest = null)
	{
		if (quest == null)
		{
			_descriptionText.text = "";
			return;
		}
		_descriptionText.text = "";

		foreach (QuestTaskSO task in quest.tasks)
		{
			if (task.curCnt < task.targetCnt)
			{
				_descriptionText.text += task.description + " [" + task.target._name + "] " + task.curCnt + " / " + task.targetCnt;
			}
			else
			{
				_descriptionText.text += task.description + " [완료] ";
			}

			_descriptionText.text += "\n"; 
		}

	}

	public void MoveToCompletedQuests(QuestSO quest)
	{
		GameObject gm;
		if (_questObject.TryGetValue(quest, out gm))
			gm.transform.SetParent(_completedQuestParent.transform);
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
			_activeQuestScrollList.SetActive(true);
			_completedQuestScrollList.SetActive(false);
		}
		else
		{
			_activeQuestScrollList.SetActive(false);
			_completedQuestScrollList.SetActive(true);
		}
	}
}
