using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class NpcGeneratorOnCondition : MonoBehaviour
{
        [SerializeField] GameObject _npc;
        [SerializeField] QuestSO _quest;
	[SerializeField] EQuestState _state; 

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
		if (_quest == null)
			return true;

		QuestManager.instance.UpdateQuestData(ref _quest);

		if (_quest.questState == _state)
		{
			transform.localScale = Vector3.one;
			Instantiate<GameObject>(_npc, transform);
			gameObject.SetActive(false);
			return true;
		}

		return false;
	}
}
