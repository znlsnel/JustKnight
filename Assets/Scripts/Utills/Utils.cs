using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Utils : Singleton<Utils>
{
        // Start is called before the first frame update

        void Start()
        {
         
        }

        // Update is called once per frame
        void Update()
        {

        }
	public void SetTimer(Action act, int FPS)
	{
		StartCoroutine(ExecuteAtFPS(act, FPS));
	}
	
	private IEnumerator ExecuteAtFPS(Action act, int FPS)
	{
		float interval = 1.0f / FPS;

		while (true)
		{
			yield return new WaitForSeconds(interval);
			act();
		}
	}
}
