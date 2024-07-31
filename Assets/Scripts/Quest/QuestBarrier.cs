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
		StartCoroutine(ClearCheck());
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		_stopUI?.SetActive(true); 
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_stopUI?.SetActive(false); 
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
				_stopUI?.SetActive(false);
				gameObject.SetActive(false);
				yield break;
			} 
		}
	}
}
