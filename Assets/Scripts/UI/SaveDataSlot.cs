using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataSlot : MonoBehaviour
{
	SaveUI _saveUI;
	SaveData _data;

	public TextMeshProUGUI _date;
	public TextMeshProUGUI _playTime;
	 
	private void Start()
	{
		_saveUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._saveUI;
		
	}
	public void onClick()
	{
		_saveUI.SelectSaveData(_data, gameObject);
	}

	public void InitSaveSlot(bool auto, SaveData saveData = null)
	{
		if (saveData == null)
			saveData =  SaveManager.instance.Save(auto);  
		_data = saveData;

		_date.text = saveData.PlayInfo._saveDate;

		int playTime = saveData.PlayInfo._playTime;
		_playTime.text = $"플레이타임 - ";

		if (playTime > 60)
			_playTime.text += (playTime / 60).ToString() + "h"; 
		if (playTime / 60 < 100)
			_playTime.text += (playTime % 60).ToString() + "m";
	}
}
