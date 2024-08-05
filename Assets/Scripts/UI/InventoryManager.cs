using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Playables;
using UnityEngine.UI;

public enum ItemType : int
{
	Headband = 0,
	Belt = 1,
	Gloves = 2,
	Shoes = 3,
	// -------------
         
	Amulet = 4,
}
 

public class Item
{
        public Item() { }
	public Item(int type, Text t)
	{
		_itemType = type;
		_name = t;
	}

	public int _itemType; 
        public Text _name;  


} 

public class InventoryManager : MonoBehaviour, IMenuUI
{ 
        [SerializeField] GameObject _inventorySlots; 
	[SerializeField] GameObject _equipSlots; 
	[SerializeField] GameObject _tempSlots;

	Text _tempSlotText;
	RectTransform _tempSlotRectTransform;

	List<Item>  _slots = new List<Item>();
        int _slotTop = 0;
        float _lastButtonDownTime = 0;

	int _hoverSlotIdx = -1;
	bool isRelocateItem = false;

	private Coroutine startCoroutine;

	private void Awake() 
	{
		_tempSlots.GetComponent<GraphicRaycaster>().enabled = false;	 
		_tempSlotText = _tempSlots.transform.Find("text").GetComponent<Text>();
		_tempSlotRectTransform = _tempSlots.GetComponent<RectTransform>();	
		foreach (Transform child in _inventorySlots.transform)
                {
			if (child.name.Contains("ivnSlot"))
			{
				int curIdx = _slots.Count;
				ButtonClickHandler bch = child.AddComponent<ButtonClickHandler>();
				if (bch != null)
				{
					bch._onButtonDown = () => OnButtonDown(curIdx);
					bch._onButtonUp = () => OnButtonUp(curIdx); 
					bch._onButtonEnter = () => OnButtonEnter(curIdx);
					bch._onButtonExit = () => OnButtonExit(curIdx);
				} 

				Text t = child.transform.Find("text")?.GetComponent<Text>();
                                if (t != null) 
                                {
					t.text= " - "; 
					_slots.Add(new Item(0, t)); 
				}
			}
                }
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (isRelocateItem)
			MoveTempSlot(); 
	}


	public void ActiveMenu(bool isActive)
        { 
                gameObject.SetActive(isActive); 
		UIHandler.instance.CloseAllUI(gameObject, isActive);
	} 

	public  void AddItem(int id)
        {
                if (_slotTop >= _slots.Count)
                        return;

                _slots[_slotTop]._itemType = id;
                _slots[_slotTop++]._name.text =  id.ToString();
        }


        void OnButtonDown(int idx)
        {
		_lastButtonDownTime = Time.time;

		startCoroutine = StartCoroutine(StartRelocateItem(idx)); 
        }

        void OnButtonUp(int idx) 
        { 
                float delay = Time.time - _lastButtonDownTime;
		if (delay < 0.2)
                {
                        _equipSlots.transform.Find("index").GetComponent<Text>().text = _slots[idx]._name.text;  
		}

		if (startCoroutine != null) 
			StopCoroutine(startCoroutine);  

		if (isRelocateItem)
		{
			isRelocateItem = false;
			_tempSlots.SetActive(false);
			RelocateItemSlot(idx);
		}
		
	} 


	void OnButtonEnter(int idx)
	{
		_hoverSlotIdx = idx; 
	}

	void OnButtonExit(int idx) 
	{
		_hoverSlotIdx = -1; 
	}

	IEnumerator StartRelocateItem(int idx)
	{
		yield return new WaitForSeconds(0.2f);
		InitTempSlot(idx);
		isRelocateItem = true;
		startCoroutine = null;
	}

	void InitTempSlot(int idx)
	{ 
		MoveTempSlot(); 
		_tempSlotText.text = _slots[idx]._name.text;
		_tempSlots.SetActive(true);
	}

	void RelocateItemSlot(int idx)
	{
		if (_hoverSlotIdx < 0)
			return;

		string tmp = _slots[idx]._name.text;
		_slots[idx]._name.text = _slots[_hoverSlotIdx]._name.text;
		_slots[_hoverSlotIdx]._name.text = tmp; 
	}

	void MoveTempSlot() 
	{
		Vector2 mousePos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_tempSlotRectTransform.parent as RectTransform, Input.mousePosition, null, out mousePos);

		_tempSlotRectTransform.anchoredPosition = mousePos; 
	
	}
}
