using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMenuUI
{
	public void ActiveMenu(bool active);  
}
public class UIHandler : Singleton<UIHandler> 
{
	// Start is called before the first frame update

	public GameObject _skillMenu;
	public GameObject _fadeEffect;
	public GameObject _mainMenu; 
	public GameObject _dialogue;
	public GameObject _displayQuest;
	public GameObject _playerUI;
	public GameObject _fpsUI;
	public GameObject _dieUI;

	GameObject _curOpenUI; 

	public override void Awake()
	{
		base.Awake();

		InstantiateAndAssign(ref _skillMenu);
		InstantiateAndAssign(ref _fadeEffect);
		InstantiateAndAssign(ref _mainMenu);
		InstantiateAndAssign(ref _dialogue);
		InstantiateAndAssign(ref _displayQuest);
		InstantiateAndAssign(ref _playerUI);
		//InstantiateAndAssign(ref _fpsUI);
		InstantiateAndAssign(ref _dieUI); 
	}

	private void InstantiateAndAssign(ref GameObject instance)
	{
		instance = Instantiate(instance);
		DontDestroyOnLoad(instance);
	} 

	private void InstantiateAndAssign<T>(ref GameObject instance, out T manager, string childName = null) where T : Component
	{
		instance = Instantiate(instance);

		manager = (childName != null) ?
			instance.transform.Find(childName).GetComponent<T>() 
			: instance.GetComponent<T>();

		DontDestroyOnLoad(instance);  
	}

	void Start()
	{ 
		InputManager.instance.BindInputAction("SkillMenu", () =>
		{
			SkillMenuManager skillMenuManager = this._skillMenu.GetComponent<SkillMenuManager>();
			skillMenuManager?.ActiveMenu(!this._skillMenu.activeSelf); 
		}); 

		InputManager.instance.BindInputAction("Inventory", () => 
		{
			this._mainMenu.GetComponent<MainMenu>().OnMenu(EMenuType.INVENTORY); 
		});

		InputManager.instance.BindInputAction("QuestMenu", () =>
		{
			this._mainMenu.GetComponent<MainMenu>().OnMenu(EMenuType.QUEST); 
		}); 
		 
	}

	public void CloseAllUI(GameObject nextUI, bool Active)
	{
		if (_curOpenUI != null)
			_curOpenUI.SetActive(false);

		if (Active)
			_curOpenUI = nextUI;
		else
			_curOpenUI = null;
	}

	public bool isOpenAnyUI()
	{
		return _curOpenUI != null; 
	}

	void Update()
    {
        
    }
}
