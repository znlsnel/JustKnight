using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[Serializable] public class SkillData
{
        public int ID;
        public string name; 
        public string description;
        public int[] value;
	public int[] coolTime;
}

[Serializable] public class SkillDatas
{
        public SkillData[] skillDatas;
}



public class SkillPopupManager : MonoBehaviour
{
        // Start is called before the first frame update
        [SerializeField] Text _skillDescription;
        [SerializeField] Text _skillName;
        [SerializeField] Text _skillLevel;
        [SerializeField] Text _skillCoolTime;
        [SerializeField] Text _skillValue;

	Canvas _myCanvas;

	public Action<int, bool> _onSkillButton; 
        public int curSkillID = 0;
        private SkillData[] _skillDatas;

	private void Awake()
	{
		_myCanvas = transform.GetComponent<Canvas>();
		LoadJsonSkillData();
	}

	void Start() 
        {

	}

        void LoadJsonSkillData()
        { 
                TextAsset jsonText = Resources.Load<TextAsset>("Datas/JS_SkillData");

		SkillDatas s = JsonUtility.FromJson<SkillDatas>(jsonText.text);
                _skillDatas = s.skillDatas; 

	}

	// Update is called once per frame
	void Update() 
        {
     
	}
         
        public void UpdateSkillId(int id)
        {
                curSkillID = id;

		_skillDescription.text = _skillDatas[curSkillID].description;
                _skillName.text = _skillDatas[curSkillID].name;  

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
