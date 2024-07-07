using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
        [SerializeField] Text _titleText;
	[SerializeField] Text _descriptionText;

	[SerializeField] Text _monsterName;
	[SerializeField] Text _huntCount;

	[SerializeField] Text _npcName;
	[SerializeField] Text _itemName;
	[SerializeField] Text _itemCount; 

	// Start is called before the first frame update
	private void Awake()
	{
		_titleText.text = "";
		_descriptionText.text = "";
		//_monsterName.text = "";
		//_huntCount.text = "";
		//_npcName.text = ""; 
		//_itemName.text = "";
		//_itemCount.text = "";
		gameObject.SetActive(false); 
	}
	void Start()
	{
		
	}

    // Update is called once per frame
    void Update() 
    {
        
    }  
	 /*
	public void UpdateQuestInfo(Quest quest)
	{
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

	}*/
}
