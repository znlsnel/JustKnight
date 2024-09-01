using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class NpcGenerator : MonoBehaviour
{
        [SerializeField] GameObject _npc;

	SpriteRenderer _spriteRenderer;
	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.enabled = false; 
	}

	public void GenNPC()
	{
		transform.localScale = Vector3.one;
		_npc.SetActive(true);
		_npc.GetComponent<DisappearOnCondition>()?.StartCheck();
			   
		gameObject.SetActive(false);
	}
}
