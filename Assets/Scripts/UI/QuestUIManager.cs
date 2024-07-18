using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour, IMenuUI
{
        [SerializeField] Text _descriptionText;

	[SerializeField] GameObject _contents;
	[SerializeField] GameObject _questPrefab;
	[SerializeField] GameObject _successUI;

	QuestSuccessUIManager _successUIManager;
	// Start is called before the first frame update

	Dictionary<QuestSO, QuestSlotManager> _slotManagers = new Dictionary<QuestSO, QuestSlotManager>();
	private void Awake()
	{
		_descriptionText.text = "";
		_successUI = Instantiate<GameObject>(_successUI);
		_successUIManager = _successUI.GetComponent<QuestSuccessUIManager>();

		DontDestroyOnLoad(_successUI); 
		gameObject.SetActive(false); 
	}
	void Start()
	{
	}

    // Update is called once per frame
    void Update() 
    {
        
    }  
	 
	public void AddQuest(QuestSO quest)
	{
		GameObject gm = Instantiate<GameObject>(_questPrefab);
		QuestSlotManager qsm = gm.GetComponent<QuestSlotManager>();
		_slotManagers.Add(quest, qsm);

		qsm.SetQuestSlot(quest);
		gm.transform.SetParent(_contents.transform);
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
				_descriptionText.text += task.description + " [¿Ï·á] ";

			_descriptionText.text += "\n"; 
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
}
