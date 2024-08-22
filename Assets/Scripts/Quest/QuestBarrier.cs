using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBarrier : MonoBehaviour
{
	// Start is called before the first frame update 
	public QuestSO _unlockQuest;
	public GameObject _stopUI;
	bool isClear = false;

	private void Awake()
	{
		_stopUI = Instantiate<GameObject>(_stopUI);
		_stopUI.SetActive(false);
	}
	private void Start()
	{
		QuestManager.instance.UpdateQuestData(ref _unlockQuest);
		_unlockQuest._onClear.Add(ClearCheck);

		if (_unlockQuest.isClear)
			ClearCheck(); 
	}
	

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (_unlockQuest.isClear || isClear)
			return;

		_stopUI?.SetActive(true); 
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (_unlockQuest.isClear || isClear)
		{
			if (_stopUI?.activeSelf == true)
				_stopUI?.SetActive(false);
			return; 
		}
		 
		Utils.instance.SetTimer(() => _stopUI?.SetActive(false), 0.5f); 
	}   

	 

	void ClearCheck()
	{
		isClear = true;

		if (_stopUI?.activeSelf == true)
			Utils.instance.SetTimer(() => _stopUI?.SetActive(false), 0.5f); 
		Utils.instance.SetTimer(() => gameObject.SetActive(false), 0.5f);
	}
}
