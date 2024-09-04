using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterBilgont : BossMonster
{
	[SerializeField] float dashPower = 10.0f;
    // Start is called before the first frame update
	public void AE_Pattern1()
	{
		float dir = transform.localScale.x > 0 ? 1 : -1;
		Vector3 vel = _rigid.velocity;
		vel.x = dir * dashPower;

		_rigid.velocity = vel;
	} 

	public void Pattern2()
	{
		 
	}
}
