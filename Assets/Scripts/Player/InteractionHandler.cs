using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
	// Start is called before the first frame update
	 
	Dictionary<GameObject, Action> _interactions = new Dictionary<GameObject, Action>(10);
	public GameObject _interactionUI;
	RectTransform _uiRT;
	Transform playerUIPos;
	Transform _playerUIPos 
	{ 
		get 
		{
			if (playerUIPos == null)
				playerUIPos = GameManager.instance.GetPlayer().GetComponent<PlayerController>()._uiPos;

			return playerUIPos;  
		}  
	}

	Action _onCancel;

	private void Start()
	{
		_uiRT = _interactionUI.GetComponent<RectTransform>();
		_interactionUI.SetActive(false);
	} 
	private void Update()
	{
		if (_interactionUI.activeSelf)
		{
			Vector3 pos = _playerUIPos.position; 
			// 플레이어의 월드 좌표를 스크린 좌표로 변환 
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(pos);

			// 스크린 좌표를 RectTransform의 좌표로 변환
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiRT.parent as RectTransform, screenPoint, Camera.main, out Vector2 localPoint);

			// UI 요소의 위치를 업데이트
			_uiRT.localPosition = localPoint;
		}
	}

	public void AddIAction(GameObject obj, Action action)
	{
		if (_interactions.Count == 0)
			_interactionUI.SetActive(true);

		_interactions.Add(obj, action); 
	}

	public void Remove(GameObject obj) 
	{
		_interactions.Remove(obj);

		if (_interactions.Count == 0 )
			_interactionUI.SetActive(false); 
	}

	public void Cancel()
	{
		if (_onCancel != null)
		{
			_onCancel.Invoke();
			_onCancel = null;
		}
	}  

	public void ExcuteInteraction()
	{
		if (_onCancel != null)
		{
			Cancel();
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
