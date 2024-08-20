using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
	public GameObject _saveSlotPrefab;
	

	[SerializeField] GameObject _slotParent;
	 
	[Space(10)]
	[SerializeField] Button _saveButton;
	[SerializeField] Button _loadButton;
	[SerializeField] Button _overwriteButton;

	SaveData _selected = null;


	public void OnSave(bool auto)
	{

		GameObject slot = Instantiate<GameObject>(_saveSlotPrefab);
		slot.transform.SetParent(_slotParent.transform);

		SaveDataSlot saveSlot = slot.GetComponent<SaveDataSlot>();
		saveSlot._date.text = DateTime.Now.ToString("yy.MM.dd (HH:mm)");
		if (auto)
			saveSlot._date.text = "<b>[auto]</b> " + saveSlot._date.text;

		int playTime = GameManager.instance._playTime;
		saveSlot._playTime.text = $"플레이타임 - {playTime / 60}h";

		if (playTime / 60 < 100)
			saveSlot._playTime.text += (playTime % 60).ToString();
	}

	public void OnLoad()
	{

	}

	public void OnOverWrite()
	{

	}

	public void SelectSaveData(SaveData data)
	{
		Color color = Color.white;
		if (data == _selected)
		{
			_selected = null;
			color.a = 0.5f;
		}  
		else
		{
			_selected = data;
		}
		 
		_loadButton.image.color = color;
		_overwriteButton.image.color = color;

		_loadButton.enabled = data != _selected;
		_overwriteButton.enabled = data != _selected;
	}
}
