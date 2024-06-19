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

	[NonSerialized] public FadeEffectManager _fadeEffectManager;
	[NonSerialized] public InventoryManager _inventoryManager;
	[NonSerialized] public SkillMenuManager _skillMenuManager;
	[NonSerialized] public DialogueHandler _dialogueSystem;

	public override void Awake()
	{ 
		base.Awake();
		_inventory = Instantiate<GameObject>(_inventory);
		_skillMenu = Instantiate<GameObject>(_skillMenu);
		_fadeEffect = Instantiate<GameObject>(_fadeEffect);
		_dialogue = Instantiate<GameObject>(_dialogue);
		 
		_fadeEffectManager = _fadeEffect.transform.Find("Panel").GetComponent<FadeEffectManager>();
		_inventoryManager = _inventory.GetComponent<InventoryManager>();
		_skillMenuManager = _skillMenu.GetComponent<SkillMenuManager>();

		_dialogueSystem = _dialogue.GetComponent<DialogueHandler>();
		 
		DontDestroyOnLoad(_fadeEffect);   
		DontDestroyOnLoad(_skillMenu);  
		DontDestroyOnLoad(_inventory);   
		DontDestroyOnLoad(_dialogue);    
	}
	 
	void Start()
	{ 
		InputManager.instance.ReceiveAction(InputManager.instance._onSkillMenu, () =>
		{
			this._skillMenuManager.ActiveMenu(this._skillMenu.activeSelf != true);
		}); 

		InputManager.instance.ReceiveAction(InputManager.instance._onInventory, () => 
		{ 
			this._inventoryManager.ActiveMenu(this._inventory.activeSelf != true);  
		});  
	}


    void Update()
    {
        
    }
}
