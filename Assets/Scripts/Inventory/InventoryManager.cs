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
        [SerializeField] GameObject _inventorySlots; 
	[SerializeField] GameObject _equipSlots; 
	[SerializeField] GameObject _tempSlots;

	Text _tempSlotText;

	List<Item>  _slots = new List<Item>();
        int _slotTop = 0;
        float _lastButtonDownTime = 0;

	int _hoverSlotIdx = -1;
	bool isRelocateItem = false;

	private Coroutine startCoroutine;

	private void Awake() 
	{ 
		//_tempSlots.GetComponent<GraphicRaycaster>().enabled = false;	 
		_tempSlotText = _tempSlots.transform.Find("text").GetComponent<Text>();
		_tempSlots.SetActive(false);
		foreach (Transform child in _inventorySlots.transform)
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
				Item item = child.GetComponent<Item>();
				_slots.Add(item);  
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

                _slots[_slotTop]._itemIdx = id;
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
		if (idx != _hoverSlotIdx)
			yield break;

		InitTempSlot(idx);
		isRelocateItem = true;
		startCoroutine = null;
	}

	void InitTempSlot(int idx)
	{ 
		MoveTempSlot();
		_tempSlotText.text = _slots[idx]._name.text;

		// Source RectTransform의 월드 좌표를 얻음
		Vector3 worldPosition = _slots[idx]._rectTransform.position;

		// 월드 좌표를 Target RectTransform의 로컬 좌표로 변환
		RectTransform targetRectTransform = _tempSlots.GetComponent<RectTransform>();
		Vector3 localPosition = targetRectTransform.parent.InverseTransformPoint(worldPosition);

		//Target RectTransform의 위치를 로컬 좌표로 설정
		targetRectTransform.localPosition = localPosition;
		prevPos = new Vector2(0, 0);
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

	Vector2 prevPos;
	void MoveTempSlot() 
	{
		RectTransform targetRectTransform = _tempSlots.GetComponent<RectTransform>();
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
