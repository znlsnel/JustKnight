using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public enum EQuestMenuType : int
{
	PENDING,
	INPROGRESS,
	COMPLETED,
	DISPLAYING,
}
public class QuestUI : MonoBehaviour, IMenuUI
{
	#region Serialize Fields

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
	[SerializeField] public GameObject _questParent_displaying;

	[Space(10)]
	[SerializeField] Button bt_questPending;
	[SerializeField] Button bt_questInProgress;
	[SerializeField] Button bt_questCompleted;
	[SerializeField] Button bt_questDisplaying;  

	[Space(10)]
	[SerializeField] public GameObject _questSlotPrefab;
	[SerializeField] GameObject _successUI;

	#endregion

	QuestSuccessUIManager _successUIManager;
	Dictionary<QuestSO, GameObject> _questObject = new Dictionary<QuestSO, GameObject>();
	Dictionary<EQuestMenuType, Tuple<GameObject, Button>> _questMenus = new Dictionary<EQuestMenuType, Tuple<GameObject, Button>>();

	Color _questMenuColor;
	private void Awake()  
	{
		_descriptionText.text = ""; 
		_successUI = Instantiate<GameObject>(_successUI);
		_successUIManager = _successUI.GetComponent<QuestSuccessUIManager>();

		_questMenus.Add(EQuestMenuType.PENDING, new Tuple<GameObject, Button>(_questView_Pending, bt_questPending));
		_questMenus.Add(EQuestMenuType.INPROGRESS, new Tuple<GameObject, Button>(_questView_InProgress, bt_questInProgress)); 
		_questMenus.Add(EQuestMenuType.COMPLETED, new Tuple<GameObject, Button>(_questView_completed, bt_questCompleted));
		_questMenus.Add(EQuestMenuType.DISPLAYING, new Tuple<GameObject, Button>(_questView_displaying, bt_questDisplaying));

		bt_questPending.onClick.AddListener(() => { OnQuestList(EQuestMenuType.PENDING); });
		bt_questInProgress.onClick.AddListener(() => { OnQuestList(EQuestMenuType.INPROGRESS); });
		bt_questCompleted.onClick.AddListener(() => { OnQuestList(EQuestMenuType.COMPLETED); }); 
		bt_questDisplaying.onClick.AddListener(() => { OnQuestList(EQuestMenuType.DISPLAYING); });
		_questMenuColor = bt_questCompleted.image.color;

		DontDestroyOnLoad(_successUI); 

	}

	GameObject GetQuestParent(EQuestMenuType type)
	{
		switch (type)
		{
			case EQuestMenuType.PENDING:
				return _questParent_Pending;

			case EQuestMenuType.COMPLETED:
				return _questParent_completed;

			case EQuestMenuType.DISPLAYING:
				return _questParent_displaying;

			case EQuestMenuType.INPROGRESS:
				return _questParent_InProgress;

			default: 
				return null;	
		}
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
				_descriptionText.text += "-" + task.description + "( " + task.curCnt + " / " + task.targetCnt + " )";
			else 
				_descriptionText.text += task.description + "<color=yellow> [¿Ï·á] </color>";  

			_descriptionText.text += "\n";  
		} 
	}
	

	public void AddQuest(EQuestMenuType type, QuestSO quest)
	{
		if (quest == null || _questObject.ContainsKey(quest))
			return;

		GameObject gm = Instantiate<GameObject>(_questSlotPrefab);
		gm.GetComponent<QuestSlotManager>().SetQuestSlot(quest);

		_questObject.Add(quest, gm);

		gm.transform.SetParent(GetQuestParent(type).transform);
		gm.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		
	}

	public void MoveToQuestList(EQuestMenuType type, QuestSO quest, bool display = false) 
	{
		
		GameObject gm;
		if (_questObject.TryGetValue(quest, out gm)) 
		{
			gm.transform.SetParent(GetQuestParent(type).transform); 
			gm.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);	

			if (display)
			{
				gm.GetComponent<QuestSlotManager>().AddDisplayQuest();
			}
		}  
	}

	public void DisplayingQuestCheck(QuestSO quest, bool check)
	{
		GameObject gm;
		if (_questObject.TryGetValue(quest, out gm))
		{
			gm.GetComponent<QuestSlotManager>()._checkBoxText.gameObject.SetActive(check);
		}
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


	void OnQuestList(EQuestMenuType type)
	{
		foreach (EQuestMenuType t in Enum.GetValues(typeof(EQuestMenuType)))
		{
			Tuple<GameObject, Button> tp = _questMenus[t];
			Color color = _questMenuColor;
			bool isActive = false;

			if (t == type)
			{ 
				color *= 1.2f;
				isActive = true;
			}
			tp.Item1.SetActive(isActive);
			tp.Item2.image.color = color;

		}
	}
}
