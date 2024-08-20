using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataSlot : MonoBehaviour
{
	SaveUI _saveUI;
	SaveData _data;

	public Text _date;
	public Text _playTime;

	private void Start()
	{
		_saveUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._saveUI;
		
	}
	public void onClick()
	{
		_saveUI.SelectSaveData(_data);
	}
}
