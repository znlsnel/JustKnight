using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	// Start is called before the first frame update
	[SerializeField] GameObject _playerPrefab;
	GameObject _player;
	 
	//public override void Awake() 
	//{
	//	player = Resources.Load<GameObject>("Characters/Player_Character");
	//} 


	void Start() 
	{
		//instance.player 
		_playerPrefab = Resources.Load<GameObject>("Characters/Player_Character");
		
	}

	private void InitGameScene(Scene scene, LoadSceneMode mode)
	{
		GameObject gen = GameObject.FindWithTag("PlayerGenPos");

		if (_player == null)
			_player = Instantiate<GameObject>(_playerPrefab);

		_player.transform.position = gen.transform.position;
		 
		gen.GetComponent<SpriteRenderer>().sortingOrder = -1; 

		Debug.Log("init ! ");
	}

	public void LoadScene(string sceneName)
	{
                SceneManager.LoadScene(sceneName);
		SceneManager.sceneLoaded += InitGameScene; 
		
	} 

	// Update is called once per frame
	void Update()
    {
        
    }
}
