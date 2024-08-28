using Cinemachine;
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

	[SerializeField] CinemachineVirtualCamera _virtualCamera;
	[SerializeField] CinemachineConfiner2D _confiner;

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
		DontDestroyOnLoad(_virtualCamera.gameObject);
	} 

	void Start()
	{
		GameManager.instance._onNextScene += () => { _virtualCamera.Follow = GameManager.instance.GetPlayer().transform;};
		GameManager.instance._onEveryScene += FindCameraBoundingBox;
	}

	public void FindCameraBoundingBox() 
	{
		GameObject bound = GameObject.FindWithTag("CameraBound");
		_confiner.m_BoundingShape2D = bound.GetComponent<PolygonCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		 
		  
	}
	 

	public void ShakeCamera(bool left)
	{
		if (left)
			_anim.Play("CameraShake_Left");
		else
			_anim.Play("CameraShake_Right");
	} 


	void AE_EndShake()
	{

	}
}
