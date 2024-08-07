using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
	public Text _name;
	public Image _itemIcon;

	public void SetImage(Sprite sprite)
	{
		_itemIcon.gameObject.SetActive(sprite != null);
		_itemIcon.sprite = sprite; 
	}
}
