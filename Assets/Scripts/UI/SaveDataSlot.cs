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

	public void InitSaveSlot(bool auto)
	{
		SaveData saveData =  SaveManager.instance.Save();
		_data = saveData; 

		_date.text = DateTime.Now.ToString("yy.MM.dd (HH:mm)");
		if (auto)
			_date.text = "<b>[auto]</b> " + _date.text;

		int playTime = GameManager.instance._playTime;
		_playTime.text = $"플레이타임 - ";

		if (playTime > 60)
			_playTime.text += (playTime / 60).ToString() + "h"; 
		if (playTime / 60 < 100)
			_playTime.text += (playTime % 60).ToString() + "m";
	}
}
