using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
	// Start is called before the first frame update
	[SerializeField] GameObject _playerPrefab;
	[SerializeField] GameObject _fadePanelPrefab;
	[SerializeField] GameObject _skillMenuManager; 
	 

	GameObject _player;
	GameObject _fadeCanvas; 
	Camera _camera; 
	FadePanelManager _fadePanelManager;

	private InputAction _UIInputManager;

	private void Awake()
	{
		_fadeCanvas = Instantiate<GameObject>(_fadePanelPrefab);
		_fadePanelManager = _fadeCanvas.transform.Find("Panel").GetComponent<FadePanelManager>();
		_camera = Camera.main;
		DontDestroyOnLoad(_fadeCanvas);
		DontDestroyOnLoad(_camera); 
		_skillMenuManager = Instantiate(_skillMenuManager);
		_skillMenuManager.gameObject.SetActive(false);

	} 
	   
	void OnSkillMenu(InputAction.CallbackContext context)
	{
		_skillMenuManager.GetComponent<SkillMenuManager>().ActiveMenu(_skillMenuManager.activeSelf != true); 
	}

	void Start()   
	{
		var inputActionAsset = Resources.Load<InputActionAsset>("Inputs/Input_UI");
		var actionMap = inputActionAsset.FindActionMap("UI");
		_UIInputManager = actionMap.FindAction("Skill_Menu");
		_UIInputManager.Enable();

		// KKeyAction에 대한 콜백 등록
		_UIInputManager.performed += OnSkillMenu;
	}

	public GameObject GetPlayer()
	{
		return _player;
	}
	 
	private void InitGameScene(Scene scene, LoadSceneMode mode)
	{
		GameObject gen = GameObject.FindWithTag("PlayerGenPos");
		gen.GetComponent<SpriteRenderer>().sortingOrder = -1; 
		
		if (_player == null)
		{
			_player = Instantiate<GameObject>(_playerPrefab);
			DontDestroyOnLoad(_player); 
		}
		_player.transform.position = gen.transform.position;
		_fadePanelManager.PlayFadeIn(); 
	}

	public void LoadScene(string sceneName)
	{
		_fadePanelManager.PlayFadeOut();
		_fadePanelManager._onFadeOutComplete = null; 
		_fadePanelManager._onFadeOutComplete += () =>
		{
			SceneManager.LoadScene(sceneName);
			SceneManager.sceneLoaded += InitGameScene; 
		}; 
	} 

	// Update is called once per frame
	void Update()
	{
		
	}
}
