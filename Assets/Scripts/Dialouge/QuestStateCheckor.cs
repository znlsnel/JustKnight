using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestStateCheckor : MonoBehaviour
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
			_quest._onChangeState += () =>
			{
				if (_quest._state == _state)
				{
					_action?.Invoke();
				}
			};
		
	} 
} 
