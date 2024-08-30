using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class NpcGeneratorOnCondition : MonoBehaviour
{
        [SerializeField] GameObject _npc;

        [SerializeField] QuestSO _quest;
	[SerializeField] EQuestState _questState;

	[SerializeField] EpisodeSO _episode;
	[SerializeField] EEpisodeState _episodeState; 

	SpriteRenderer _spriteRenderer;
	void Start()
	{
		StartCoroutine(UpdateCheck(Check));
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.enabled = false; 
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

		if ((_quest != null && _quest.questState == _questState) || (_episode != null && _episode._state == _episodeState) ) 
		{
			transform.localScale = Vector3.one;
			_npc.SetActive(true);
			_npc.GetComponent<DisappearOnCondition>()?.StartCheck();
			   
			gameObject.SetActive(false); 
			return true;
		}
		else if (_npc.activeSelf == true)
			_npc.SetActive(false);

		return false;
	}
}
