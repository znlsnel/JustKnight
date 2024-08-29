using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
	MainMenu _menu;
	SaveUI _saveUI;
	
	private void Start()
	{
		_menu = UIHandler.instance._mainMenu.GetComponent<MainMenu>();
		_saveUI = _menu._saveUI;

		GameManager.instance._onNextScene += () =>
		{
			_menu._backGround.SetActive(true);
			_saveUI.SetSaveButtonActive(true);
			_menu.GetComponent<Canvas>().sortingOrder = 0;
		}; 
	}

	public void OnNewGameButton()
        {
                GameManager.instance.LoadScene("Tutorial");    
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
