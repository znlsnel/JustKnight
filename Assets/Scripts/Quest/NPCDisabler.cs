using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class NPCDisabler : MonoBehaviour
{
	public void Execute() 
	{
		gameObject.SetActive(false);
	} 
}
