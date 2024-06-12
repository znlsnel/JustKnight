using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
        Button _button;

	Action _onButtonDown;
	Action _onButtonUp; 
	Action _onButtonEnter; 
	Action _onButtonExit; 

	// Start is called before the first frame update
	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	public void InitButtonUpDown(Action up, Action down, Action enter = null, Action exit = null)
	{
		_onButtonDown = down;
		_onButtonUp = up;
		 _onButtonEnter = enter; 
		 _onButtonExit = exit;
	}

    // Update is called once per frame

	public void OnPointerDown(PointerEventData eventData)
	{
		_onButtonDown?.Invoke();
	}
	 
	public void OnPointerUp(PointerEventData eventData)
	{
		_onButtonUp?.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_onButtonEnter?.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{ 
		_onButtonExit?.Invoke(); 
	}
}
