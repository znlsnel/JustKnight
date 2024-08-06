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



public class InventoryManager : MonoBehaviour, IMenuUI
{ 
        [SerializeField] GameObject _equipSlots; 
        [SerializeField] GameObject _inventorySlots; 
	[SerializeField] Image _selectItemIcon;  
	[SerializeField] Text _selectItemDiscript;
	[SerializeField] GameObject _moveSlot; 

	List<Tuple<ItemSO, GameObject>> _slots = new List<Tuple<ItemSO, GameObject>>();
	
	int _equipSlotCount = 0;
        int _slotTop = 0;
	int _hoverSlotIdx = -1;

        float _lastButtonDownTime = 0;

	bool isRelocateItem = false;

	private Coroutine startCoroutine;

	private void Awake() 
	{ 
		//_tempSlots.GetComponent<GraphicRaycaster>().enabled = false;	 
		_moveSlot.SetActive(false);

		InitSlot(_equipSlots);
		InitSlot(_inventorySlots);
		 
		gameObject.SetActive(false);
	}
	
	void InitSlot(GameObject slot)
	{
		foreach (Transform child in slot.transform)
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

			Text t = child.GetComponentInChildren<Text>(); 
			if (t != null)
			{
				t.text = " - ";
				_slots.Add(new Tuple<ItemSO, GameObject>(null, child.gameObject));  
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
	} 

	public  void AddItem(int id)
        {
                if (_slotTop >= _slots.Count)
                        return;

                //_slots[_slotTop]._itemIdx = id;
                //_slots[_slotTop++]._name.text =  id.ToString();
        }


        void OnButtonDown(int idx)
        {
		Debug.Log("Button Down");
		_lastButtonDownTime = Time.time;

		startCoroutine = StartCoroutine(StartRelocateItem(idx));
		
        }

        void OnButtonUp(int idx) 
        { 

                float delay = Time.time - _lastButtonDownTime;
		if (delay < 0.2)
                {
			Sprite newSprite = Sprite.Create(_slots[idx].Item1._itemIcon,
				 new Rect(0, 0, _selectItemIcon.sprite.texture.width, _selectItemIcon.sprite.texture.height),
				 _selectItemIcon.sprite.pivot);
			 
			_selectItemIcon.sprite = newSprite;
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

		_hoverSlotIdx = idx;
	}

	void OnButtonExit(int idx) 
	{
		_hoverSlotIdx = -1;
	}

	IEnumerator StartRelocateItem(int idx)
	{
		yield return new WaitForSeconds(0.2f);
		if (idx != _hoverSlotIdx)
			yield break;

		InitTempSlot(idx);
		isRelocateItem = true;
		startCoroutine = null;
	}

	void InitTempSlot(int idx)
	{ 
		MoveTempSlot();
		_moveSlot.GetComponentInChildren<Text>().text = "이동중";
		//_moveSlot.GetComponentInChildren<Text>().text = _slots[idx]._name.text;
		 
		// Source RectTransform의 월드 좌표를 얻음
		Vector3 worldPosition = _slots[idx].Item2.GetComponent<RectTransform>().position;
		 
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
		if (_hoverSlotIdx < 0)
			return;

		Tuple<ItemSO, GameObject> nextDst = new Tuple<ItemSO, GameObject>(_slots[idx].Item1, _slots[_hoverSlotIdx].Item2);
		Tuple<ItemSO, GameObject> nextSouce = new Tuple<ItemSO, GameObject>(_slots[_hoverSlotIdx].Item1, _slots[idx].Item2);

		_slots[idx] = nextSouce;
		_slots[_hoverSlotIdx] = nextDst; 
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
}
