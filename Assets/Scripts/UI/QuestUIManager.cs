using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
        [SerializeField] Text _descriptionText;

	[SerializeField] GameObject _contents;
	[SerializeField] GameObject _questPrefab; 
	// Start is called before the first frame update
	private void Awake()
	{
		_descriptionText.text = "";

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

       public void UpdateQuestInfo(QuestSO quest)
	{

		// 몬스터를 사냥해주세요 {이름} 0 / 25;
		_descriptionText.text = quest.description + quest.task.target._name + quest.task.curCnt + " / " + quest.task.targetCnt;
		/*
	       _titleText.text = quest.title;
	       _descriptionText.text = quest.description;

	       _monsterName.gameObject.transform.parent.gameObject.SetActive(quest.monsterHunt.monsterName != null);
	       _npcName.gameObject.transform.parent.gameObject.SetActive(quest.delivery.itemName != null); 

	       if (quest.delivery.itemName != null)
	       {
		       _npcName.text = quest.delivery.npcName;
		       _itemName.text = quest.delivery.itemName;
		       _itemCount.text = "0 / " + quest.delivery.itemCount.ToString(); 
	       } 

	       else if (quest.monsterHunt.monsterName != null) 
	       {
		       _monsterName.text = quest.monsterHunt.monsterName;
		       _huntCount.text = quest.monsterHunt.huntCount.ToString(); 

	       }
		*/
	}
}
