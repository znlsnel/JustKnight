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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		_stopUI?.SetActive(true); 
		StartCoroutine(ClearCheck()); 
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		Utils.instance.SetTimer(() => _stopUI?.SetActive(false), 0.5f);
	}   

	

	IEnumerator ClearCheck()
	{
		while (true)
		{
			yield return new WaitForSeconds(1.0f);
			_unlockQuest = QuestManager.instance.UpdateQuestData(_unlockQuest);
			isClear = _unlockQuest.isClear();

			if (isClear)
			{
				Utils.instance.SetTimer(() => _stopUI?.SetActive(false), 0.5f);
				Utils.instance.SetTimer(() => gameObject.SetActive(false), 1.0f);  
				  
				yield break;
			} 
		}
	}
}
