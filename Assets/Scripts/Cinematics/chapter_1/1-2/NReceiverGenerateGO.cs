using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class NReceiverGenerateGO : MonoBehaviour, INotificationReceiver
{
	public void OnNotify(Playable origin, INotification notification, object context)
	{
		if (notification is MK_GenerateGameObject gen)
		{
			Debug.Log("이벤트 수신: ");
			// 빌곤트 등장 로직 실행
			TriggerBilgontAppearance(gen._prefab, gen._transfom);
		}
	}

	void TriggerBilgontAppearance(GameObject prefab, Transform tf)
	{
		Instantiate<GameObject>(prefab, tf);
	}
}
