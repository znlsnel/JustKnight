using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Utils : Singleton<Utils>
{
	public Coroutine SetTimer(Action act, float second)
	{
		return StartCoroutine(Execute(act, second));
	}
	 
	private IEnumerator Execute(Action act, float second)
	{
		yield return new WaitForSeconds(second); 
		act.Invoke(); 
	}

} 
 