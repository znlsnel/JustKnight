using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMenuUI
{
	public void ActiveMenu(bool active);  
}
public class UIHandler : Singleton<UIHandler> 
{
	// Start is called before the first frame update

	[SerializeField] GameObject _skillMenu;
	[SerializeField] GameObject _fadeEffect;
	[SerializeField] GameObject _inventory; 
	[SerializeField] GameObject _dialogue;
	[SerializeField] GameObject _questMenu;
	[SerializeField] GameObject _displayQuest;
	[SerializeField] GameObject _playerUI;

	[NonSerialized] public FadeEffectManager _fadeEffectManager;
	[NonSerialized] public InventoryManager _inventoryManager;
	[NonSerialized] public SkillMenuManager _skillMenuManager;
	[NonSerialized] public DialogueManager _dialogueSystem;
	[NonSerialized] public QuestUI _questUIManager;
	[NonSerialized] public DisplayQuest _displayQuestManager;
	[NonSerialized] public PlayerUI _playerUIManager;

	GameObject _curOpenUI;

	public override void Awake()
	{
		base.Awake();
		InstantiateAndAssign(ref _inventory, out _inventoryManager);
		InstantiateAndAssign(ref _skillMenu, out _skillMenuManager);
		InstantiateAndAssign(ref _fadeEffect, out _fadeEffectManager, "Panel");
		InstantiateAndAssign(ref _dialogue, out _dialogueSystem); 
		InstantiateAndAssign(ref _questMenu, out _questUIManager);
		InstantiateAndAssign(ref _displayQuest, out _displayQuestManager);
		InstantiateAndAssign(ref _playerUI, out _playerUIManager); 

	}
	private void InstantiateAndAssign<T>(ref GameObject instance, out T manager, string childName = null) where T : Component
	{
		instance = Instantiate(instance);

		manager = (childName != null) ?
			instance.transform.Find(childName).GetComponent<T>() 
			: instance.GetComponent<T>();

		DontDestroyOnLoad(instance);  
	}

	void Start()
	{ 
		InputManager.instance.BindInputAction("SkillMenu", () =>
		{
			this._skillMenuManager.ActiveMenu(!this._skillMenu.activeSelf);
		}); 

		InputManager.instance.BindInputAction("Inventory", () => 
		{ 
			this._inventoryManager.ActiveMenu(!this._inventory.activeSelf);
		});

		InputManager.instance.BindInputAction("QuestMenu", () =>
		{ 
			this._questUIManager.ActiveMenu(!_questMenu.activeSelf); 
		});

	}

	public void CloseAllUI(GameObject nextUI, bool Active)
	{
		if (_curOpenUI != null)
			_curOpenUI.SetActive(false);

		if (Active)
			_curOpenUI = nextUI;
		else
			_curOpenUI = null;
	}

	public bool isOpenAnyUI()
	{
		return _curOpenUI != null; 
	}

	void Update()
    {
        
    }
}
