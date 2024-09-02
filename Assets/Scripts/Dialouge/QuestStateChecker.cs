using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestStateChecker : MonoBehaviour
{
	[SerializeField] QuestSO _quest;
	[SerializeField] EQuestState _state;

	public UnityEvent _action;

	private void Start()
	{
		QuestManager.instance.UpdateQuestData(ref _quest);
		if (_quest._state == _state)
			_action?.Invoke();
		else
			_quest._onChangeState += ACTION;
	}
	 
	void ACTION()
	{
		if (_quest._state == _state)
		{ 
			_action?.Invoke();
			_quest._onChangeState -= ACTION;
		}
	}
} 
