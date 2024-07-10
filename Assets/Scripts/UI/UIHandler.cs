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

	[NonSerialized] public FadeEffectManager _fadeEffectManager;
	[NonSerialized] public InventoryManager _inventoryManager;
	[NonSerialized] public SkillMenuManager _skillMenuManager;
	[NonSerialized] public DialogueHandler _dialogueSystem;
	[NonSerialized] public QuestUIManager _questUIManager;

	GameObject _curOpenUI;

	public override void Awake()
	{
		base.Awake();
		_inventory = Instantiate<GameObject>(_inventory);
		_skillMenu = Instantiate<GameObject>(_skillMenu);
		_fadeEffect = Instantiate<GameObject>(_fadeEffect);
		_dialogue = Instantiate<GameObject>(_dialogue);
		_questMenu = Instantiate<GameObject>(_questMenu);

		_fadeEffectManager = _fadeEffect.transform.Find("Panel").GetComponent<FadeEffectManager>();
		_inventoryManager = _inventory.GetComponent<InventoryManager>();
		_skillMenuManager = _skillMenu.GetComponent<SkillMenuManager>();
		_dialogueSystem = _dialogue.GetComponent<DialogueHandler>();
		_questUIManager = _questMenu.GetComponent<QuestUIManager>(); 
		 

		DontDestroyOnLoad(_fadeEffect);   
		DontDestroyOnLoad(_skillMenu);  
		DontDestroyOnLoad(_inventory);   
		DontDestroyOnLoad(_dialogue);    
		DontDestroyOnLoad(_questMenu);      
	}
	 
	void Start()
	{ 
		InputManager.instance.BindInputAction(InputManager.instance._onSkillMenu, () =>
		{
			this._skillMenuManager.ActiveMenu(!this._skillMenu.activeSelf);
		}); 

		InputManager.instance.BindInputAction(InputManager.instance._onInventory, () => 
		{ 
			this._inventoryManager.ActiveMenu(!this._inventory.activeSelf);
		});

		InputManager.instance.BindInputAction(InputManager.instance._onQuestMenu, () =>
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
