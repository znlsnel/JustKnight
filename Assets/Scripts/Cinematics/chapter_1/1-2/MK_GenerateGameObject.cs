using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MK_GenerateGameObject : Marker, INotification
{
	public GameObject _prefab;
	public Transform _transfom;
	public PropertyName id => throw new System.NotImplementedException();
}
