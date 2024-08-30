using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisappearOnCondition : MonoBehaviour
{
	// Start is called before the first frame update

	[SerializeField] QuestSO _quest;


	void Start()
        {
		StartCoroutine(UpdateCheck(Check));
	}

	IEnumerator UpdateCheck(Func<bool> func)
	{
		while (true)
		{
			if (func.Invoke())
				break;

			yield return new WaitForSeconds(0.5f);
		}
	}
	 
        bool Check()
        {
		if (_quest == null)
			return true;

		QuestManager.instance.UpdateQuestData(ref _quest);

		if (_quest.isClear)
		{
			gameObject.SetActive(false);
			return true;
		}
		
		return false;
	}
}
