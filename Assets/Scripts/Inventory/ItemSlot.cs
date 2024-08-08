using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
	public Text _name;
	public Image _itemIcon;
	public Image _itemEffectIcon;

	public void SetImage(Sprite sprite, ItemSO item = null)
	{
		_itemIcon.gameObject.SetActive(sprite != null);
		_itemIcon.sprite = sprite;

		_itemEffectIcon.gameObject.SetActive(item != null && item._effects.Count >= 3); 
		if (item != null)
		{
			int level = item._effects.Count;

			if (level == 10)
				_itemEffectIcon.color = Color.black;

			else if(level >= 8)
				_itemEffectIcon.color = Color.red;

			else if(level >= 5)
				_itemEffectIcon.color = Color.yellow;

			else if (level >= 3)
				_itemEffectIcon.color = Color.blue; 
		}
	}
}
