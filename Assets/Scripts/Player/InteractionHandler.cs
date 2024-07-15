using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
	// Start is called before the first frame update
	 
	Dictionary<GameObject, Action> _interactions = new Dictionary<GameObject, Action>(10);
	Action _onCancel;
	public void AddIAction(GameObject obj, Action action)
	{
		_interactions.Add(obj, action);
	}

	public void Remove(GameObject obj) 
	{
		_interactions.Remove(obj);
	}
	 
	public void ExcuteInteraction()
	{
		if (_onCancel != null)
		{
			_onCancel?.Invoke();
			_onCancel = null;
			return;
		}

		if (_interactions.Count == 0)
		{
			Debug.Log("상호작용할 NPC가 주변에 없습니다."); 
			return;
		}

		Vector3 playerPos = GameManager.instance.GetPlayer().transform.position;
		float curDist = 100000.0f;
		Action ac = null;

		foreach (var action in _interactions)
		{
			float dist = (action.Key.transform.position - playerPos).magnitude;
			if (curDist > dist)
			{
				curDist = dist;
				ac = action.Value;
			}
		}

		ac?.Invoke(); 
	}

	public void RegisterCancelAction(Action action)
	{
		_onCancel = null;
		_onCancel += action; 
	}

}
