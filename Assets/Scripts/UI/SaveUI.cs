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
	[SerializeField] Button _deleteButton;

	SaveData _selected = null;
	GameObject _selectedObject;

	float lastSaveTime = 0.0f;
	private void Start()
	{
		SelectSaveData(null);
	}

	public void OnSave(bool auto)
	{
		if (Time.time - lastSaveTime < 1.01)
			return;
		lastSaveTime = Time.time;	 

		OnSave(auto, null );  
	}
	public void OnSave(bool auto, SaveData saveData)
	{
		lastSaveTime = Time.time;

		GameObject slot = Instantiate<GameObject>(_saveSlotPrefab);
		slot.transform.SetParent(_slotParent.transform);
		slot.transform.localScale = Vector3.one;
		slot.transform.SetSiblingIndex(0); 

		slot.GetComponent<SaveDataSlot>().InitSaveSlot(auto, saveData);
	}


	public void OnLoad()
	{
		 
	}

	public void OnOverWrite()
	{
		SaveData saveData = SaveManager.instance.Save(false, _selected.fileName);
		_selectedObject.GetComponent<SaveDataSlot>().InitSaveSlot(false, saveData); 
	}

	public void OnDelete()
	{
		Debug.Log(_selected.fileName);
		SaveManager.instance.DeleteSaveFile(_selected.fileName);
		_selected = null;
		SelectSaveData(null);
		_selectedObject.transform.localScale = Vector3.one;
		Destroy(_selectedObject);
	}

	public void SelectSaveData(SaveData data, GameObject obj = null)
	{
		Color color = Color.white;
		Vector3 size = Vector3.one;

		if (_selectedObject != null)
			_selectedObject.transform.localScale = size;

		if (data == _selected)
		{
			_selected = null;
			color.a = 0.5f;
		}
		else
		{
			_selected = data;
			size *= 0.9f;
			_selectedObject = obj;
		}

		 
		if (obj != null)
			obj.transform.localScale = size;

		
		_loadButton.image.color = color;
		_overwriteButton.image.color = color;
		_deleteButton.image.color = color;

		_loadButton.enabled = _selected != null;
		_overwriteButton.enabled = _selected != null;
		_deleteButton.enabled = _selected != null; 
	}
}
