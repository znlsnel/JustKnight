using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.IO;





public class SkillPopupManager : MonoBehaviour
{
        // Start is called before the first frame update
        [SerializeField] public Text _skillDescription;
        [SerializeField] public Text _skillName;
        [SerializeField] public Text _skillState;


	Canvas _myCanvas;

	public Action<int, bool> _onSkillButton; 
        public int curSkillID = 0;
       

	private void Awake()
	{
		_myCanvas = transform.GetComponent<Canvas>();
	}

	void Start() 
        {

	}

	public void SetSkillStateText(int level, int value, int coolTime)
	{
		_skillState.text = "";
		_skillState.text += "Level : " + (level == 0 ? '-' : level.ToString()) + "\n";
		if (value > 0)
			_skillState.text += "value : " + value.ToString() + "\n";
		if (coolTime > 0)
			_skillState.text += "coolTime : " + coolTime.ToString();   
	}

	// Update is called once per frame
	void Update() 
        {
     
	}
         
        public void UpdateSkillId(int id)
        {
                curSkillID = id;

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
