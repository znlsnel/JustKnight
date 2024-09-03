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

		_virtualCamera.Follow = GameManager.instance.GetPlayer().transform;

		PolygonCollider2D bound = FindObjectOfType<PolygonCollider2D>();
		_confiner.m_BoundingShape2D = null; 
		_confiner.m_BoundingShape2D = bound;

		float value = 1.2f * GameManager.instance.GetPlayer().transform.localScale.x;
		CinemachineFramingTransposer framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		framingTransposer.m_TrackedObjectOffset.x = value;

		// Cinemachine Transposer 가져오기
		var transposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

		if (transposer != null)
		{
			// 카메라가 한 번에 이동하도록 Damping을 0으로 설정
			float x = transposer.m_XDamping;
			float y  = transposer.m_YDamping;
			float z = transposer.m_ZDamping;

			transposer.m_XDamping = 0.0f;
			transposer.m_YDamping = 0.0f;
			transposer.m_ZDamping = 0.0f;

			// 원하는 위치로 카메라 이동

			Utils.instance.SetTimer(() =>
			{
				transposer.m_XDamping = x;
				transposer.m_YDamping = y;
				transposer.m_ZDamping = z;
			}, 0.1f);
		}


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
