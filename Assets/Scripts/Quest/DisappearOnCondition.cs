using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class DisappearOnCondition : MonoBehaviour
{
	// Start is called before the first frame update

	[SerializeField] QuestSO _quest;
	[SerializeField] EpisodeSO _episode; 
	[SerializeField] EEpisodeState _episodeState;
	 
	public void StartCheck()  
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
		if (_quest != null)
			QuestManager.instance.UpdateQuestData(ref _quest);
		if (_episode != null)
			UIHandler.instance._dialogue.GetComponent<DialogueManager>().UpdateQuestDialogue(ref _episode);

		if ((_quest != null && _quest.isClear) || (_episode != null && _episode._state == _episodeState))
		{ 
			gameObject.SetActive(false); 
			return true;
		}
		
		return false;
	}
}
