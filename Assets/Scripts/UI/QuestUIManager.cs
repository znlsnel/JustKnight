using System.Collections;
using System.Collections.Generic;
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
		gm.GetComponentInChildren<Text>().text = quest.name;
		gm.transform.SetParent(_contents.transform);

		ButtonClickHandler b = gm.AddComponent<ButtonClickHandler>();
		b.RegisterButtonAction(() => { OnButtonDown(quest); }, () => { OnButtonUp(quest); });
	}
	public void OnButtonDown(QuestSO quest)
	{

	} 

	public void OnButtonUp(QuestSO quest)
	{
		UpdateQuestInfo(quest);
	} 

	// if you don't send anything as a factor, this function will empty the quest text
       public void UpdateQuestInfo(QuestSO quest = null)
	{
		if (quest == null)
		{
			_descriptionText.text = "";
			return;
		}

		if (quest.task.curCnt == quest.task.targetCnt)
			_descriptionText.text = "CLEAR !!";
		else 
			_descriptionText.text = quest.description + " [" + quest.task.target._name +"] "+ quest.task.curCnt + " / " + quest.task.targetCnt;
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
