using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
	// Start is called before the first frame update
	public Collider2D _sensorFrontTop;
	public Collider2D _sensorFrontBottom;
	public Collider2D _sensorBackBottom;
	public Collider2D _sensorGround;
	public Collider2D _sensorGroundFar; 
	public Collider2D _sensorAttack;

	public bool _onSensorFT { get { return CheckSensor(_sensorFrontTop); } } 
	public bool _onSensorFB { get { return CheckSensor(_sensorFrontBottom); } }
	public bool _onSensorBB { get { return CheckSensor(_sensorBackBottom); } }
	public bool _onSensorGround { get { return CheckSensor(_sensorGround); } }
	public bool _onSensorGroundFar { get { return CheckSensor(_sensorGroundFar); } }

	bool CheckSensor(Collider2D collider)
	{
		ContactFilter2D contactFilter = new ContactFilter2D();
		if (collider == null)
			return false; 
		 
		Collider2D[] result = new Collider2D[10];
		int count = Physics2D.OverlapCollider(collider, contactFilter, result);


		foreach (Collider2D r in result)
		{
			if (r == null || r.gameObject.layer != LayerMask.NameToLayer("Tile"))
				continue; 
			 
			if (r.gameObject != gameObject)
			{ 
				return true;
			} 
		}

		return false;
	}



}
