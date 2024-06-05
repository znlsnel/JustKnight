using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : Singleton<UIHandler> 
{
	// Start is called before the first frame update

	[SerializeField] GameObject _skillMenu;
	[SerializeField] GameObject _fadeEffect;

	[NonSerialized] public FadeEffectManager _fadeEffectManager;   
	 
	public override void Awake()
	{ 
		base.Awake(); 
		_skillMenu = Instantiate(_skillMenu);
		_skillMenu.gameObject.SetActive(false); 

		_fadeEffect = Instantiate<GameObject>(_fadeEffect);
		_fadeEffectManager = _fadeEffect.transform.Find("Panel").GetComponent<FadeEffectManager>();

		DontDestroyOnLoad(_fadeEffect);  
	}

	void Start()
	{
		InputManager.instance.ReceiveAction(InputManager.instance._onSkillMenu, () =>
		{
			this._skillMenu.GetComponent<SkillMenuManager>().ActiveMenu(this._skillMenu.activeSelf != true);
		});
		 
	//	InputManager.instance._onSkillMenu.performed += (InputAction.CallbackContext context) =>
	//	{
	//		this._skillMenu.GetComponent<SkillMenuManager>().ActiveMenu(this._skillMenu.activeSelf != true);
	//	};  
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
