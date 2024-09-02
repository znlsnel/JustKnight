using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class NpcGenerator : MonoBehaviour
{
        [SerializeField] GameObject _npc;

	private void Awake()
	{ 
	//	_npc.SetActive(false);
	}

	void Start()
	{
		_npc.SetActive(false);
	}

	public void GenNPC()
	{
		transform.localScale = Vector3.one;
		_npc.SetActive(true);			   
		gameObject.SetActive(false); 
	}
}
