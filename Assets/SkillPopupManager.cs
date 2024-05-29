using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPopupManager : MonoBehaviour
{
        Canvas _myCanvas;
        // Start is called before the first frame update

        public Action<int, bool> _onSkillButton; 
        public int curSkillID = 0;

        void Start()
        {
	        _myCanvas = transform.GetComponent<Canvas>();
        }

    // Update is called once per frame
        void Update() 
        {
        
        }
         
       public void OnLevelUpButton()
        {
                _onSkillButton.Invoke(curSkillID, true);
	}

	public void OnLevelDownButton()
        {
		_onSkillButton.Invoke(curSkillID, false);
	}

	public void OnQuitButton()
        {
		Debug.Log(" QUIT BUTTON");
                _myCanvas.gameObject.SetActive(false);
	}
}
