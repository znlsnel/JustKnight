using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EMenuType : int
{
	INVENTORY = 0,
	QUEST,

	COUNT,
}

public class MainMenu : MonoBehaviour
{
        public GameObject _inventory;
        public GameObject _quest;

	Dictionary<EMenuType, Tuple<GameObject, Button>> _menuButtons = new Dictionary<EMenuType, Tuple<GameObject, Button>>();
        public Button bt_onInventory;
        public Button bt_onQuest;

	[NonSerialized] public InventoryManager _inventoryManager;
	[NonSerialized] public QuestUI _questUI;
	 

	private void Awake()
	{
		_inventoryManager = _inventory.GetComponent<InventoryManager>(); 
		_questUI = _quest.GetComponent<QuestUI>(); 

		_menuButtons.Add(EMenuType.INVENTORY, new Tuple<GameObject, Button>(_inventory, bt_onInventory));
		_menuButtons.Add(EMenuType.QUEST, new Tuple<GameObject, Button>(_quest, bt_onQuest));

		bt_onInventory.onClick.AddListener(() => OnMenu(EMenuType.INVENTORY));
		bt_onQuest.onClick.AddListener(() => OnMenu(EMenuType.QUEST));

		gameObject.SetActive(false);
	} 

	public void OnMenu(EMenuType type)
	{
		if (gameObject.activeSelf == false)
		{
			gameObject.SetActive(true);
			InputManager.instance.FreezeCharacter(true);
		}

		for (EMenuType t = EMenuType.INVENTORY; t < EMenuType.COUNT; t++)
		{
			Tuple<GameObject, Button> tp = _menuButtons[t];
			Color color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
			bool isActive = false;

			if (t == type)
			{
				color = Color.white;
				isActive = true;
			}

			tp.Item1.SetActive(isActive); 
			tp.Item2.image.color = color; 
		}
	}

	public void CloseMenu()
	{
		gameObject.SetActive(false);  
		InputManager.instance.FreezeCharacter(false);
	}
}
