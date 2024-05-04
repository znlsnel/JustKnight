using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	// Start is called before the first frame update
	[SerializeField] GameObject _playerPrefab;
	[SerializeField] GameObject _fadePanelPrefab;  
	GameObject _player;
	GameObject _fadeCanvas;
	FadePanelManager _fadePanelManager;

	void Start() 
	{
		_fadeCanvas = Instantiate<GameObject>(_fadePanelPrefab);
		_fadePanelManager = _fadeCanvas.transform.Find("FadePanel").GetComponent<FadePanelManager>();
		DontDestroyOnLoad(_fadeCanvas); 
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
