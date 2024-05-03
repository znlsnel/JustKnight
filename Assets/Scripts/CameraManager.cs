using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
        // Start is called before the first frame update 
       GameObject _player;
	public static BackGroundManager _backGroundManager;
    void Start()
    {
		_player = GameObject.FindWithTag("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
		 

	}

	private void LateUpdate() 
	{
		float nextPosX = _player.transform.position.x;
		float curPosX = transform.position.x;
		float dir = nextPosX - curPosX;
		 
		Vector3 curPos = transform.position; 
		curPos.x = nextPosX; 
		transform.position = curPos;
		 
		_backGroundManager.UpdateBackgroundPos(dir);
	}
}
