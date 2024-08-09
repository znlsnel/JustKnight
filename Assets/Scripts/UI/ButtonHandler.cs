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
