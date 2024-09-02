using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class NReceiverGenerateGO : MonoBehaviour, INotificationReceiver
{
	[SerializeField] GameObject _prefab;
	[SerializeField] Transform _transform;

	public void OnNotify(Playable origin, INotification notification, object context)
	{
		Debug.Log("이벤트 수신: ");
		if (notification is MK_GenerateGameObject gen)
		{
			Debug.Log("이벤트 수신: ");
			// 빌곤트 등장 로직 실행
			TriggerBilgontAppearance();
		}
	}

	void TriggerBilgontAppearance()
	{
		Instantiate<GameObject>(_prefab, _transform); 
	}
}
