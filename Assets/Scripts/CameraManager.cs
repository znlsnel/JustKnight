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
		GameManager.instance._onNextScene += () => { 
			_virtualCamera.Follow = GameManager.instance.GetPlayer().transform;
			GameManager.instance.GetPlayer().GetComponent<PlayerMovementController>()._onPlayerLookDirChanged += (int dir) =>
			{
				float value = 1.2f * dir;

				CinemachineFramingTransposer framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
				framingTransposer.m_TrackedObjectOffset.x = value;
			};
		};
		GameManager.instance._onEveryScene += InitCamera; 
	}


	public void InitCamera()  
	{
		Camera[] allCameras = FindObjectsOfType<Camera>();
		foreach(Camera cam in allCameras)
		{
			if (cam.gameObject != gameObject)
				Destroy(cam.gameObject);
		}

		PolygonCollider2D bound = FindObjectOfType<PolygonCollider2D>();
		_confiner.m_BoundingShape2D = null; 
		_confiner.m_BoundingShape2D = bound;

		float value = 1.2f * GameManager.instance.GetPlayer().transform.localScale.x;
		CinemachineFramingTransposer framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		framingTransposer.m_TrackedObjectOffset.x = value;
	}


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
