using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputManager : Singleton<InputManager>
{
	[NonSerialized] public InteractionHandler _interactionHandler;

	private Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();
	private InputActionMap _uiActionMap;
	private InputActionMap _CharacterActionMap;

	// Start is called before the first frame update
	public override void Awake() 
	{
		base.Awake();

		_interactionHandler = gameObject.AddComponent<InteractionHandler>();

		var inputActionAsset = Resources.Load<InputActionAsset>("Inputs/InputActions");
		_uiActionMap = inputActionAsset.FindActionMap("UI");
		_CharacterActionMap = inputActionAsset.FindActionMap("Character");

		foreach (InputActionMap actionMap in inputActionAsset.actionMaps)
		{
			foreach (InputAction action in actionMap.actions)
			{
				_inputActions.Add(action.name, action);
				action.Enable();
			}
		}
	}

	void Start() 
	{
		BindInputAction("Interaction", () => {_interactionHandler.ExcuteInteraction();});
	}

	public void BindInputAction(string inputActionName, Action act, Action cancel = null)
	{
		InputAction inputAction;
		if (_inputActions.TryGetValue(inputActionName, out inputAction))
		{
			 Action< InputAction.CallbackContext> action = (InputAction.CallbackContext context) => { act.Invoke(); };
			inputAction.performed += action; 
			if (cancel != null)
			{
				Action<InputAction.CallbackContext> cc = (InputAction.CallbackContext context) => { cancel.Invoke(); };
				inputAction.canceled += cc;
			}
		} 
	}

	public InputAction GetInputAction(string inputActionName)
	{
		return _inputActions[inputActionName];
	}

	// Update is called once per frame
	void Update()
	{

	}

}
