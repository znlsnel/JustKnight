using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
	// Start is called before the first frame update
	public Collider2D _sensorFrontTop;
	Collider2D _sensorFrontBottom;
	Collider2D _sensorBackBottom;
	Collider2D _sensorGround;
	Collider2D _sensorGroundFar;
	public Collider2D _sensorAttack;

	public bool _onSensorFT = false;
	public bool _onSensorFB = false;
	public bool _onSensorBB = false;
	public bool _onSensorGround = false;
	public bool _onSensorGroundFar = false;

	private void Awake()
	{
		_sensorFrontTop = transform.Find("CollisionSensor_FrontTop").GetComponent<CircleCollider2D>();
		_sensorFrontBottom = transform.Find("CollisionSensor_FrontBottom").GetComponent<CircleCollider2D>();
		_sensorBackBottom = transform.Find("CollisionSensor_BackBottom").GetComponent<CircleCollider2D>();
		_sensorGround = transform.Find("CollisionSensor_Ground").GetComponent<Collider2D>();
		_sensorGroundFar = transform.Find("CollisionSensor_Ground2").GetComponent<Collider2D>();
		_sensorAttack = transform.Find("CollisionSensor_Attack").GetComponent<Collider2D>();
	}
	void Start()
	{
		Utils.instance.SetTimer(() => { CheckSensor(); }, 30);
	}

	// Update is called once per frame
	void Update()
	{
		
		 

	}

	void CheckSensor()
	{
		ContactFilter2D contactFilter = new ContactFilter2D();

		Func<Collider2D, bool> check = (Collider2D collider) =>
		{
			if (collider == null)
				return false;

			bool returnVal = false;
			Collider2D[] result = new Collider2D[2];
			int count = Physics2D.OverlapCollider(collider, contactFilter, result);


			for (int i = 0; i < count; i++)
			{
				if (returnVal == false)
					returnVal = result[i].gameObject != gameObject;
			}

			return returnVal;
		};

		_onSensorFT = check(_sensorFrontTop);
		_onSensorFB = check(_sensorFrontBottom);
		_onSensorBB = check(_sensorBackBottom);
		_onSensorGround = check(_sensorGround);
		_onSensorGroundFar = check(_sensorGroundFar); 
	}



}
