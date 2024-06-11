using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputManager : Singleton<InputManager>
{ 
	[NonSerialized] public InputAction _onSkillMenu;
	[NonSerialized] public InputAction _onInventory;

	// Start is called before the first frame update
	public override void Awake() 
	{
		base.Awake();

		var inputActionAsset = Resources.Load<InputActionAsset>("Inputs/Input_UI");
		var actionMap = inputActionAsset.FindActionMap("UI");
		_onInventory = actionMap.FindAction("Inventory");
		_onSkillMenu = actionMap.FindAction("Skill_Menu");
		_onSkillMenu.Enable();  
		_onInventory.Enable();
	}
	void Start()
	{

		 

	}
	
	public void ReceiveAction(InputAction input, Action action)
	{
		input.performed += (InputAction.CallbackContext context) => 
		{ 
			action.Invoke();  
		}; 
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
