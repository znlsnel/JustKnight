using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class NReceiverGenerateGO : MonoBehaviour, INotificationReceiver
{
	[SerializeField] GameObject _go;
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

	public void TriggerBilgontAppearance()
	{
		_go.SetActive(true);
		_go.transform.position = _transform.position; 
	}
}
