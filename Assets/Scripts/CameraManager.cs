using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene; 

public class CameraManager : MonoBehaviour
{
	static bool isAwake = false;
        // Start is called before the first frame update 
       GameObject _player;
	Animator _anim;
	private void Awake()
	{
		if (isAwake)
		{ 
			Destroy(gameObject);
			return;
		}

		isAwake = true;
		_anim = GetComponent<Animator>();	
	} 

	void Start()
	{

	}

    // Update is called once per frame
	void Update()
	{
		 
		  
	}
	 

	public void ShakeCamera_Left()
	{
		
		_anim.Play("CameraShake_Left");

	}
	public void ShakeCamera_Right()
	{
		_anim.Play("CameraShake_Right");  
	}

	void AE_EndShake()
	{
	//	test = false;
	}

	private void LateUpdate()     
	{ 
	//	if (test) return; 
		if (_player == null )
		{
			_player = GameManager.instance.GetPlayer();
			return;
		}
		 
		float nextPosX = _player.transform.position.x;
		float curPosX = transform.position.x;
		float dir = nextPosX - curPosX;
		 
		Vector3 curPos = transform.position; 
		curPos.x = nextPosX; 
		transform.position = curPos;	 
	}


}
