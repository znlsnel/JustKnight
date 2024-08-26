using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Utils : Singleton<Utils>
{
	public Coroutine SetTimer(Action act, float second = 0.0f)
	{
		return StartCoroutine(Execute(act, second));
	}
	 
	private IEnumerator Execute(Action act, float second)
	{
		if (second == 0.0f)
			yield return new WaitForEndOfFrame();
		else
			yield return new WaitForSeconds(second); 

		act.Invoke();  
	}

} 
 