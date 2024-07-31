using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Utils : Singleton<Utils>
{
	public void SetTimer(Action act, float second)
	{
		StartCoroutine(Execute(act, second));
	}
	 
	private IEnumerator Execute(Action act, float second)
	{
		yield return new WaitForSeconds(second); 
		act.Invoke(); 
	}
} 
 