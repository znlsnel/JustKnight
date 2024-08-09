using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Playables;
using UnityEngine.UI;



public class InventoryManager : MonoBehaviour, IMenuUI
{ 
        [SerializeField] GameObject _equipSlots; 
        [SerializeField] GameObject _inventorySlots;
	[Space(10)]

	[SerializeField] Image _selectItemIcon;
	[SerializeField] Text _selectItemTitle;
	[SerializeField] Text _selectItemDescript;
	[SerializeField] Text _statusText;

	[Space(10)]
	[SerializeField] GameObject _moveSlot;
	[SerializeField] GameObject _deletePopup;
	[SerializeField] GameObject _wastebasket;
	[SerializeField] GameObject _status;


	List<ItemSO> _items;
	List<GameObject> _slots = new List<GameObject> ();

	int _equipSlotCount = 0;
	int _itemSlotCnt = 0;
	int _usedSlotCnt = 0;
	int _horverSlotIdx = -1;

        float _lastButtonDownTime = 0;

	bool isRelocateItem = false;

	private Coroutine startCoroutine;

	private  void Awake() 
	{
		_moveSlot.GetComponent<GraphicRaycaster>().enabled = false;	 
		_moveSlot.SetActive(false);

		InitSlot(_equipSlots); 
		_equipSlotCount = _slots.Count;
		InitSlot(_inventorySlots); 
		_itemSlotCnt = (_slots.Count - _equipSlotCount) - 1;

		_items = new List<ItemSO>(_slots.Count);
		for (int i = 0; i < _slots.Count; i++)
			_items.Add(null);

		gameObject.SetActive(false); 
	}

	
	void InitSlot(GameObject slot)
	{
		foreach (Transform child in slot.transform)
		{
			int curIdx = _slots.Count;
			ButtonHandler bch = child.AddComponent<ButtonHandler>();
			if (bch != null)
			{
				bch._onButtonDown = () => OnButtonDown(curIdx);
				bch._onButtonUp = () => OnButtonUp(curIdx);
				bch._onButtonEnter = () => OnButtonEnter(curIdx);
				bch._onButtonExit = () => OnButtonExit(curIdx);
			}

			Text t = child.GetComponentInChildren<Text>(); 
			if (t != null)
			{
				t.text = " - ";
				_slots.Add(child.gameObject);  
			}

		}
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

		_status.SetActive(false);
		_deletePopup.SetActive(false);	 
		
	} 

	public bool isEmpty()
	{
		return _usedSlotCnt == _itemSlotCnt; 
	}

	public  void AddItem(ItemSO item)
        {
                if (_usedSlotCnt >= _itemSlotCnt)
                        return;

		for (int i = _equipSlotCount; i <  _slots.Count; i++)
		{

			if (_items[i] == null && _slots[i] != _wastebasket)
			{
				_usedSlotCnt++; 
				_items[i] = item;

				item.InitItem();
				Sprite newSprite = Sprite.Create(item._itemIcon,
					 new Rect(0, 0, item._itemIcon.width, item._itemIcon.height),
					 new Vector2(0.5f, 0.5f));

				_slots[i].GetComponent<ItemSlot>().SetImage(newSprite, item);

				return;
			}
		}
        }


        void OnButtonDown(int idx)
        {
		if (_items[idx] == _wastebasket)
			return;

		_lastButtonDownTime = Time.time;

		if (_items[idx] != null)
			startCoroutine = StartCoroutine(StartRelocateItem(idx));
		
        }

        void OnButtonUp(int idx) 
        {


		float delay = Time.time - _lastButtonDownTime;
		if (delay < 0.2)
                { 
			Sprite nextSprite = null;
			_selectItemDescript.text = "";
			_selectItemTitle.text = "";
			if (_items[idx] != null)
			{
				nextSprite = Sprite.Create(_items[idx]._itemIcon,
				 new Rect(0, 0, _items[idx]._itemIcon.width, _items[idx]._itemIcon.height),
				 new Vector2(0.5f, 0.5f));
				_selectItemTitle.text = _items[idx]._name;
				_selectItemDescript.text = _items[idx]._description;
			}
			_selectItemIcon.gameObject.SetActive(nextSprite != null);
			_selectItemDescript.gameObject.SetActive(nextSprite != null);
			_selectItemTitle.gameObject.SetActive(nextSprite != null);
			_selectItemIcon.sprite = nextSprite;


		}

		if (startCoroutine != null) 
			StopCoroutine(startCoroutine);  

		if (isRelocateItem)
		{
			isRelocateItem = false;
			_moveSlot.SetActive(false);
			RelocateItemSlot(idx);
		}
		
	} 


	void OnButtonEnter(int idx)
	{


		_horverSlotIdx = idx;
	}

	void OnButtonExit(int idx) 
	{

		_horverSlotIdx = -1; 
	}

	IEnumerator StartRelocateItem(int idx)
	{
		yield return new WaitForSeconds(0.2f);
		if (idx != _horverSlotIdx)
			yield break; 

		InitTempSlot(idx);
		isRelocateItem = true;
		startCoroutine = null;
	}

	void InitTempSlot(int idx)
	{ 
		MoveTempSlot();

		Sprite newSprite = Sprite.Create(_items[idx]._itemIcon,
			 new Rect(0, 0, _items[idx]._itemIcon.width, _items[idx]._itemIcon.height),
			 new Vector2(0.5f, 0.5f));

		_moveSlot.GetComponentInChildren<ItemSlot>().SetImage( newSprite);
		//_moveSlot.GetComponentInChildren<Text>().text = _slots[idx]._name.text;

		// Source RectTransform의 월드 좌표를 얻음
		Vector3 worldPosition = _slots[idx].GetComponent<RectTransform>().position;
		 
		// 월드 좌표를 Target RectTransform의 로컬 좌표로 변환
		RectTransform targetRectTransform = _moveSlot.GetComponent<RectTransform>();
		Vector3 localPosition = targetRectTransform.parent.InverseTransformPoint(worldPosition);

		//Target RectTransform의 위치를 로컬 좌표로 설정
		targetRectTransform.localPosition = localPosition;
		prevPos = new Vector2(0, 0);
		_moveSlot.SetActive(true);
	}

	void RelocateItemSlot(int idx)
	{
		if (_horverSlotIdx < 0)
			return;
		
		if (_slots[_horverSlotIdx] == _wastebasket)
		{
			RemoveItem(idx);
			return;
		}

		if (idx >= _equipSlotCount && _horverSlotIdx < _equipSlotCount)
		{
			_usedSlotCnt--;
			EquipItem(idx);
			EquipItem(_horverSlotIdx, true);
		}
		else if (idx < _equipSlotCount && _horverSlotIdx >= _equipSlotCount)
		{
			EquipItem(idx, true);
			EquipItem(_horverSlotIdx);  
		}


		ItemSO temp = _items[_horverSlotIdx];
		_items[_horverSlotIdx] = _items[idx];
		_items[idx] = temp;
		
		Sprite tempS = _slots[idx].GetComponent<ItemSlot>()._itemIcon.sprite;
		_slots[idx].GetComponent<ItemSlot>().SetImage(_slots[_horverSlotIdx].GetComponent<ItemSlot>()._itemIcon.sprite, _items[idx]);
		_slots[_horverSlotIdx].GetComponent<ItemSlot>().SetImage( tempS, _items[_horverSlotIdx]); 

	}  
	void EquipItem(int idx, bool unequip = false)
	{
		if (unequip)
			_items[idx]?.UnequipItem();
		else
			_items[idx]?.EquipItem();

		_statusText.text = PlayerEffectManager.instance.GetStatus();

	}

	Vector2 prevPos;
	void MoveTempSlot() 
	{
		RectTransform targetRectTransform = _moveSlot.GetComponent<RectTransform>();
		Vector3 localPosition = targetRectTransform.parent.InverseTransformPoint(Input.mousePosition);
		Vector2 mousePos = localPosition;
		if (prevPos.magnitude == 0)
		{
			prevPos = mousePos;
			return;
		}
		 
		targetRectTransform.anchoredPosition += mousePos - prevPos ;
		prevPos = mousePos;
	}

	Action _onRemoveItem;

	void RemoveItem(int idx)
	{
		_deletePopup.SetActive(true);
		_onRemoveItem = () =>
		{
			if (idx < _equipSlotCount)
				EquipItem(idx, true);
			else
				_usedSlotCnt--;

			_items[idx] = null;
			_slots[idx].GetComponent<ItemSlot>().SetImage(null);
		};
	}

	public void OnRemoveItemButton()
	{
		_onRemoveItem.Invoke(); 
		_deletePopup.SetActive(false);
	}

	public void OnRemoveCancelButton()
	{
		_deletePopup.SetActive(false);

	}

	public void OnStatus()
	{
		_statusText.text = PlayerEffectManager.instance.GetStatus();

		_status.SetActive(true);
	}

	public void CloseStatus()
	{
		_status.SetActive(false);

	}


}
