using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;

public class EpisodeStateCheckor : MonoBehaviour
{
	[SerializeField] EpisodeSO _episode;
	[SerializeField] EEpisodeState _state;

	public UnityEvent _action;

	private void Start()
	{
		 UIHandler.instance._dialogue.GetComponent<DialogueManager>().UpdateQuestDialogue(ref _episode);

		if (_episode._state == _state)
			_action?.Invoke();
		else
			_episode._onChangeState += () =>
			{
				if (_episode._state == _state)
				{
					_action?.Invoke();
				}
			};
		
		 
	}  
}