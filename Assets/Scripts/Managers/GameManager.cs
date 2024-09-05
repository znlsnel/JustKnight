using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;

public class GameManager : Singleton<GameManager>
{

	// Start is called before the first frame update
	[SerializeField] GameObject _playerPrefab;
	GameObject _player; 
	Camera _camera;
	public Action _onNextScene;
	public Action _onEveryScene;
	FadeEffectManager _fadeEffect;

	[NonSerialized] public int _playTime;
	InventoryManager _inventory;
	DialogueManager _dialogue;
	QuestManager _quest;
	AudioReporter _audioReporter;


	public override void Awake()
	{
		base.Awake();

		_camera = Camera.main;
		DontDestroyOnLoad(_camera);
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitGameScene;
	}

	IEnumerator UpdateTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(60);
			++_playTime;
		}
	}


	void Start()   
	{
		_inventory = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._inventoryManager;
		_dialogue = UIHandler.instance._dialogue.GetComponent<DialogueManager>();
		_audioReporter = GetComponent<AudioReporter>();
		_quest = QuestManager.instance;

		_fadeEffect = UIHandler.instance._fadeEffect.GetComponentInChildren<FadeEffectManager>();
		StartCoroutine(UpdateTime());

		//_audioReporter.PlayAnimationSound(0);  
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
		_fadeEffect.PlayFadeIn();


		_onNextScene?.Invoke();
		_onNextScene = null;
		_onEveryScene?.Invoke();


	}

	public void LoadScene(string sceneName) 
	{
		_fadeEffect.PlayFadeOut();
		_fadeEffect._onFadeOutComplete = null;

		_fadeEffect._onFadeOutComplete += () =>
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		}; 
	}

	public void ResetGame()
	{
		_inventory.ResetInventory();
		_dialogue.ResetEpisode();
		_quest.ResetQuest();
	}
	// Update is called once per frame
	void Update()
	{
		
	}
}
