using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputManager : Singleton<InputManager>
{ 
	public InputAction _onSkillMenu;

	// Start is called before the first frame update
	public override void Awake()
	{
		base.Awake();

	}
	void Start()
	{
		var inputActionAsset = Resources.Load<InputActionAsset>("Inputs/Input_UI");
		var actionMap = inputActionAsset.FindActionMap("UI");
		_onSkillMenu = actionMap.FindAction("Skill_Menu");
		_onSkillMenu.Enable();
		 

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
