using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

	public Action _onButtonDown;
	public Action _onButtonUp;
	public Action _onButtonEnter;
	public Action _onButtonExit;

	// Start is called before the first frame update

	// Update is called once per frame

	float lastButtonDown = 0.0f;
	public void OnPointerDown(PointerEventData eventData)
	{
		_onButtonDown?.Invoke();
		lastButtonDown = Time.time; 
	}
	 
	public void OnPointerUp(PointerEventData eventData)
	{
//		if (Time.time  - lastButtonDown < 0.1)
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
