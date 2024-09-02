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
		Debug.Log("�̺�Ʈ ����: ");
		if (notification is MK_GenerateGameObject gen)
		{
			Debug.Log("�̺�Ʈ ����: ");
			// ����Ʈ ���� ���� ����
			TriggerBilgontAppearance();
		}
	}

	void TriggerBilgontAppearance()
	{
		Instantiate<GameObject>(_prefab, _transform); 
	}
}
