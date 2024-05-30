using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillData
{
        public int ID;
        public string name; 
        public string description;
        public int[] value;
	public int[] coolTime;
}

[Serializable]
public class SkillDatas
{
        public SkillData[] skillDatas;
}
public class SkillPopupManager : MonoBehaviour
{


        Canvas _myCanvas; 
        // Start is called before the first frame update

        public Action<int, bool> _onSkillButton; 
        public int curSkillID = 0;

        void Start()
        {
	        _myCanvas = transform.GetComponent<Canvas>();
                LoadJsonSkillInfo();

	}

        void LoadJsonSkillInfo()
        { 
                TextAsset jsonText = Resources.Load<TextAsset>("Datas/JS_SkillData");

		SkillDatas s = JsonUtility.FromJson<SkillDatas>(jsonText.text); 

                Debug.Log(s.skillDatas.Length);

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
