using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : Singleton<StartMenu>
{
	MainMenu _menu;
	SaveUI _saveUI;
	public override void Awake()
	{
		base.Awake();
	}
	private void Start()
	{
		_menu = UIHandler.instance._mainMenu.GetComponent<MainMenu>();
		_saveUI = _menu._saveUI;

		GameManager.instance._onEveryScene += () =>
		{
			_menu._backGround.SetActive(true);
			_saveUI.SetSaveButtonActive(true);
			_menu.GetComponent<Canvas>().sortingOrder = 0;
			ActiveStartMenu(false);
		}; 

	
	}

	public void ActiveStartMenu(bool active)
	{
		gameObject.SetActive(active);
	}

	public void OnNewGameButton()
	{
		// 완전 초기화
		GameManager.instance.LoadScene("Tutorial");
		GameManager.instance._onNextScene += () =>
		{
			GameManager.instance.ResetGame();

			Utils.instance.SetTimer(() =>
			{
				PlayerController pc = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
				pc.hp = PlayerStats.instance.GetValue(EPlayerStatus.HP);
				GameManager.instance._playTime = 0;
			}, 0.1f);
		};
	} 

	public void OnLoadGame()
	{
		_menu.OnMenu(EMenuType.SAVE);
		_menu._backGround.SetActive(false);

		_saveUI.SetSaveButtonActive(false);
		_menu.GetComponent<Canvas>().sortingOrder = 2;
	}

	public void OnGameQuit()
        {
		Application.Quit();

		// 유니티 에디터에서 플레이 모드 종료
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif

	}
}
