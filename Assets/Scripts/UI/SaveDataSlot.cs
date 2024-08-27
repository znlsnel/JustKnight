using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataSlot : MonoBehaviour
{
	public Action _onSlotClick;
	public SaveData _data;

	public TextMeshProUGUI _date;
	public TextMeshProUGUI _playTime;
	 
	public void onClick()
	{
		
		_onSlotClick?.Invoke();
	}

	public void InitSaveSlot(SaveData saveData, Action act = null)
	{
		if (act != null)
			_onSlotClick = act;

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
