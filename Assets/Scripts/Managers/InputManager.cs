using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputManager : Singleton<InputManager>
{
	public InteractionHandler _interactionHandler;
	[NonSerialized] public InputAction _onSkillMenu;
	[NonSerialized] public InputAction _onInventory;
	[NonSerialized] public InputAction _onInteraction; 

	// Start is called before the first frame update
	public override void Awake() 
	{
		base.Awake();

		var inputActionAsset = Resources.Load<InputActionAsset>("Inputs/Input_UI");
		var actionMap = inputActionAsset.FindActionMap("UI");
		_onInventory = actionMap.FindAction("Inventory");
		_onSkillMenu = actionMap.FindAction("Skill_Menu");
		_onInteraction = actionMap.FindAction("Interaction");
		_onSkillMenu.Enable();  
		_onInventory.Enable();
		_onInteraction.Enable();


		_interactionHandler = gameObject.AddComponent<InteractionHandler>(); 
	}
	void Start() 
	{
		ReceiveAction(_onInteraction, () => {

			if (_interactionHandler == null)
			{
				Debug.Log(" Exception _interactionHandler is Null"); 
				return;
			}
			_interactionHandler.ExcuteInteraction(); 
		
		});
		


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
