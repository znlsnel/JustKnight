using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[Serializable]
public class SkillData
{
	public int ID;
	public string name;
	public string description;
	public int[] value;
	public int[] coolTime;
	public int maxLevel = 0;

	public void Init()
	{
		int vLen = value == null ? 0 : value.Length;
		int cLen = coolTime == null ? 0 : coolTime.Length;


		 
		maxLevel = math.max(vLen, cLen);
	}

}

[Serializable]
public class SkillDatas
{
	public SkillData[] skillDatas;
}

public class SkillMenuManager : MonoBehaviour, IMenuUI 
{
	[SerializeField] Text _skillPointText;
	[SerializeField] Canvas _skillPopupMenu; 

        [SerializeField]Button[] _skillButton = new Button[8];
	Text[] _skillLevelTexts = new Text[8];
	int[] _skillLevels = new int[8];
	 
	Canvas canvas;
	Canvas _upgradeMenu;  
	SkillPopupManager _skillPopupManager;

	int skillPoint = 5;

	private SkillData[] _skillDatas;

	private void Awake() 
	{ 
		canvas = GetComponent<Canvas>();

                _skillPopupMenu = Instantiate(_skillPopupMenu);
		_skillPopupMenu.gameObject.SetActive(false);
		gameObject.SetActive(false);
		_skillPopupManager = _skillPopupMenu.GetComponent<SkillPopupManager>();
		_skillPopupManager._onSkillButton = OnSkillButton; 

		DontDestroyOnLoad(_skillPopupManager); 
	}

	// Start is called before the first frame update
	void Start()
        {
		_skillPointText.text = skillPoint.ToString();

		for (int i = 0; i < _skillButton.Length; i++)
                {
                        int k = i;   
                        _skillButton[i].onClick.AddListener(() => {
				ClickButton(k);    
                        });  
                }
		LoadJsonSkillData();

	}
	void LoadJsonSkillData()
	{
		TextAsset jsonText = Resources.Load<TextAsset>("Datas/JS_SkillData");

		SkillDatas s = JsonUtility.FromJson<SkillDatas>(jsonText.text);
		_skillDatas = s.skillDatas;
		foreach (SkillData skillData in _skillDatas) 
			skillData.Init(); 

	}

	public void OpenUI()
        {
                gameObject.SetActive(true);
        }
        public void CloseUI()
        {
                gameObject.SetActive(false);

	}

	// 0 , 1, 2
	// 3, 4, 5
	// 6, 7

	static int[] parent = new int[8] { 
		-1, -1, -1, 0, 1, 2, 3, 4 
	};
	static int[] childScore = new int[8]
	{
		0, 0, 0, 0, 0, 0, 0, 0
	};

	public Vector2 test = new Vector2(100.0f, 100.0f); 
	void ClickButton(int id)
        {
		_skillPopupManager.UpdateSkillId(id);
		_skillPopupMenu.gameObject.SetActive(true);
		UpdatePopupInfo(id);


	}

	void OnSkillButton(int skillId, bool isUpgrade)
        {
		if (isUpgrade)
		{
			if (parent[skillId] != -1 && _skillLevels[parent[skillId]] == 0)
				return; 

			if (skillPoint == 0 || _skillDatas[skillId].maxLevel <= _skillLevels[skillId]) 
				return;



			if (_skillLevels[skillId] == 0)
			{
				_skillButton[skillId].transform.Find("OffMode").gameObject.SetActive(false);
				Transform gm = _skillButton[skillId].transform.Find("check");
				Transform icon = gm.Find("checkIconColor");
				_skillLevelTexts[skillId] = gm.Find("Frame").Find("SkillLevel").GetComponent<Text>();

				icon.GetComponent<Image>().color = new Color(102.0f / 255.0f, 132 / 255.0f, 1.0f, 1.0f);
			}

			_skillLevelTexts[skillId].text = (++_skillLevels[skillId]).ToString();

			if (parent[skillId] != -1)
				childScore[parent[skillId]]++; 

			--skillPoint;
			_skillPointText.text = skillPoint.ToString(); 
		} 
		else
		{
			Debug.Log($"Child Score : {childScore[skillId]}, Skil Level : {_skillLevels[skillId]}"); 
			if (_skillLevels[skillId] <= 0 ||( childScore[skillId] > 0 && _skillLevels[skillId] == 1))
				return; 
			  
			string str = (--_skillLevels[skillId]).ToString();
			_skillLevelTexts[skillId].text = _skillLevels[skillId] == 0 ? " " : str;
			if (parent[skillId] != -1)
				childScore[parent[skillId]]--; 
			 
			++skillPoint;
			_skillPointText.text = skillPoint.ToString();

			Debug.Log($"Level : {_skillLevels[skillId]}"); 

			if (_skillLevels[skillId] == 0)
			{
				_skillButton[skillId].transform.Find("OffMode").gameObject.SetActive(true);
				Transform gm = _skillButton[skillId].transform.Find("check");
				Transform icon = gm.Find("checkIconColor");
				_skillLevelTexts[skillId] = gm.Find("Frame").Find("SkillLevel").GetComponent<Text>();

				icon.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f); 
			}
		}
		 
		UpdatePopupInfo(skillId);

	}  
	void UpdatePopupInfo(int skillId)
	{
		_skillPopupManager._skillDescription.text = _skillDatas[skillId].description;
		_skillPopupManager._skillName.text = _skillDatas[skillId].name;
		

		int level = _skillLevels[skillId];
		if (_skillDatas[skillId].maxLevel <= level)
			level = 1000;

		int value = -1;
		int ctm = -1; 
		if (level > 0)
		{
			bool hasValue = _skillDatas[skillId].value != null;
			 value = hasValue ? _skillDatas[skillId].value[math.min(level - 1, _skillDatas[skillId].value.Length - 1)] : -1;
			bool hasCtm = _skillDatas[skillId].coolTime != null;
			 ctm = hasCtm ? _skillDatas[skillId].coolTime[math.min(level - 1, _skillDatas[skillId].coolTime.Length - 1)] : -1;
		}

		_skillPopupManager.SetSkillStateText(level, value, ctm);
	} 

	public void ActiveMenu(bool isOpen)
	{
		if (isOpen)
		{
			gameObject.SetActive(true); 
			_skillPopupMenu.gameObject.SetActive(false); 
		}
		else
		{
			_skillPopupMenu.gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
	}

}
