using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
	// Start is called before the first frame update

	PlayerController _playerController;
	[NonSerialized] public Animator anim;
	private string curAnim = ""; 

	void Start()
	{
		anim = GetComponent<Animator>();
		_playerController = GetComponent<PlayerController>();
		PlayAnimation();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public float GetCurAnimLength()
	{
		return anim.GetCurrentAnimatorStateInfo(0).length;
	}
	public void PlayAnimation(string animName = "") 
	{ 
		if (animName == "")
		{
			switch (_playerController._playerState)
			{
				case EPlayerState.Idle:
					animName = "Idle";
					break;
				case EPlayerState.Move:
					animName = "Run";
					break;
				case EPlayerState.Fall:
					animName = "Fall";
					break;
				case EPlayerState.Death:
					animName = "Death";
					break;
				default:
					break;
			} 
		}
		 
		anim.Play(animName);   
	}

}
