using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EMenuType : int
{
	INVENTORY = 0,
	QUEST,
	SAVE,
}

public class MainMenu : MonoBehaviour
{
	public GameObject _backGround;
        public GameObject _inventory;
        public GameObject _quest;
        public GameObject _save;
	

	Dictionary<EMenuType, Tuple<GameObject, Button>> _menuButtons = new Dictionary<EMenuType, Tuple<GameObject, Button>>();
        public Button bt_onInventory;
        public Button bt_onQuest;
        public Button bt_onSave;

	[NonSerialized] public InventoryManager _inventoryManager;
	[NonSerialized] public QuestUI _questUI;
	[NonSerialized] public SaveUI _saveUI;

	Color _buttonColor;

	private void Awake()
	{ 
		_inventory.SetActive(true);
		_quest.SetActive(true);
		_save.SetActive(true); 

		_inventoryManager = _inventory.GetComponent<InventoryManager>(); 
		_questUI = _quest.GetComponent<QuestUI>();
		_saveUI = _save.GetComponent<SaveUI>();

		bt_onInventory.onClick.AddListener(() => OnMenu(EMenuType.INVENTORY));
		bt_onQuest.onClick.AddListener(() => OnMenu(EMenuType.QUEST));
		bt_onSave.onClick.AddListener(() => OnMenu(EMenuType.SAVE));

		_buttonColor = bt_onInventory.image.color; 

		_menuButtons.Add(EMenuType.INVENTORY, new Tuple<GameObject, Button>(_inventory, bt_onInventory));
		_menuButtons.Add(EMenuType.QUEST, new Tuple<GameObject, Button>(_quest, bt_onQuest));
		_menuButtons.Add(EMenuType.SAVE, new Tuple<GameObject, Button>(_save, bt_onSave)); 


	}

	public void OnMenu(EMenuType type)
	{
		if (gameObject.activeSelf == false)
		{
			gameObject.SetActive(true);
			InputManager.instance.FreezeCharacter(true);
		}

		foreach (EMenuType t in Enum.GetValues(typeof(EMenuType)))
		{
			Tuple<GameObject, Button> tp = _menuButtons[t];
			Color color = _buttonColor; 
			bool isActive = false;

			if (t == type)
			{
				color *= 1.1f;
				isActive = true;
			}

			tp.Item1.gameObject.SetActive(isActive);
			tp.Item2.image.color = color;
		}
	}

	public void CloseMenu()
	{
		gameObject.SetActive(false);  
		InputManager.instance.FreezeCharacter(false);
	}

	private void Start()
	{
		gameObject.SetActive(false);

	}
}
