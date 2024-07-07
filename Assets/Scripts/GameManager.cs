using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : Singleton<GameManager>
{

	// Start is called before the first frame update
//	MonsterGenerator _monsterGenerator;
	[SerializeField] GameObject _playerPrefab;
	GameObject _player; 
	Camera _camera;
	public Action _onSceneInit;

	public override void Awake() 
	{
	        base.Awake();

		//_monsterGenerator = gameObject.AddComponent<MonsterGenerator>();
		_camera = Camera.main;
		DontDestroyOnLoad(_camera);
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitGameScene;

	}


	void Start()   
	{


	}

	public GameObject GetPlayer()
	{
		 
		return _player;
	} 
	 
	private void InitGameScene(Scene scene, LoadSceneMode mode)
	{ 
		GameObject gen = GameObject.FindWithTag("PlayerGenPos");
		if (gen == null)
			return;

		gen.GetComponent<SpriteRenderer>().sortingOrder = -1; 
		
		if (_player == null)
		{
			_player = Instantiate<GameObject>(_playerPrefab);
			DontDestroyOnLoad(_player); 
		}
		_player.transform.position = gen.transform.position;
		UIHandler.instance._fadeEffectManager.PlayFadeIn();

		_onSceneInit?.Invoke();
	}
	   
	public void LoadScene(string sceneName) 
	{ 
		UIHandler.instance._fadeEffectManager.PlayFadeOut();
		UIHandler.instance._fadeEffectManager._onFadeOutComplete = null;

		UIHandler.instance._fadeEffectManager._onFadeOutComplete += () =>
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		}; 
	} 

	// Update is called once per frame
	void Update()
	{
		
	}
}
