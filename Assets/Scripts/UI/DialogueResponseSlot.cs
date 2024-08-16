using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueResponseSlot : MonoBehaviour
{
        public TextMeshProUGUI _text; 
	ButtonHandler buttonHandler;

	[SerializeField] Color _pressColor;
	[SerializeField] Color _hoverColor;

	private void Awake()
	{ 
		Color normal = _text.color;
		buttonHandler = gameObject.AddComponent<ButtonHandler>();
		{
			buttonHandler._onButtonDown = () => { _text.color = _pressColor; };
			buttonHandler._onButtonUp = () => { _text.color = normal; };
			buttonHandler._onButtonEnter = () => { _text.color = _hoverColor; };
			buttonHandler._onButtonExit = () => { _text.color = normal; }; 
		}
	}

	public void InitResponseText(GameObject parent, string text, Action _onClick)
	{
		_text.text = text;
		gameObject.transform.SetParent(parent.transform);
		buttonHandler._onButtonUp += _onClick.Invoke;  
	}
}
