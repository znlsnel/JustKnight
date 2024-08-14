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
	[SerializeField] GameObject _interactionUIPrefab;
	public InteractionHandler _interactionHandler{get { return _interactionUIPrefab.GetComponent<InteractionHandler>(); }}
	 
	private Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();
	private InputActionMap _uiActionMap;
	private InputActionMap _characterActionMap;

	// Start is called before the first frame update 
	public override void Awake() 
	{
		base.Awake(); 
		_interactionUIPrefab = Instantiate<GameObject>(_interactionUIPrefab);

		DontDestroyOnLoad(_interactionUIPrefab);
		 
		var inputActionAsset = Resources.Load<InputActionAsset>("Inputs/InputActions");
		_uiActionMap = inputActionAsset.FindActionMap("UI");
		_characterActionMap = inputActionAsset.FindActionMap("Character");

		foreach (InputActionMap actionMap in inputActionAsset.actionMaps)
		{
			foreach (InputAction action in actionMap.actions)
			{
				_inputActions.Add(action.name, action); 
				action.Enable();
			}
		}
	}
	public void Freezz(bool freeze)
	{
		//FreezeUI(freeze);
		FreezeCharacter(freeze);
	}
	public void FreezeUI(bool freeze)
	{
		if (freeze)
		{
			_uiActionMap.Disable();
		}
		else
			_uiActionMap.Enable();
	}

	public void FreezeCharacter(bool  freeze)
	{
		if (freeze)
		{
			_characterActionMap.Disable();
			Rigidbody2D rigid = GameManager.instance.GetPlayer().GetComponent<PlayerMovementController>()._rigidbody;
			Vector3 temp = rigid.velocity;
			temp.x /= 2.0f; 
			rigid.velocity = temp;
		}
		else
			_characterActionMap.Enable();

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
		if (_inputActions.ContainsKey(inputActionName))
			return _inputActions[inputActionName];

		Debug.Log($"Input Action을 찾지 못했습니다. [{inputActionName}]");
		return null;
	}

	// Update is called once per frame
	void Update()
	{

	}

}
