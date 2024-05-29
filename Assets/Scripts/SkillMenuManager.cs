using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SkillMenuManager : MonoBehaviour
{
	[SerializeField] Canvas _skillPopupMenu; 
        [SerializeField]Button[] _skillButton = new Button[8];
        [SerializeField] Text _skillPointText;

	Canvas canvas;
	Canvas _upgradeMenu;  
	SkillPopupManager _skillPopupManager;
	Text[] _skillLevelTexts = new Text[8];
        int[] _skillLevels = new int[8];
	int skillPoint = 5;
         


	private void Awake()
	{ 
		canvas = GetComponent<Canvas>();
                _skillPopupMenu = Instantiate(_skillPopupMenu);
		_skillPopupMenu.gameObject.SetActive(false);
		_skillPopupManager = _skillPopupMenu.GetComponent<SkillPopupManager>();
		_skillPopupManager._onSkillButton = OnSkillButton; 
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
		_skillPopupManager.curSkillID = id;
		_skillPopupMenu.gameObject.SetActive(true); 

	//	int MaxX = 320;
	//	int MinX = -320;
	//	int MaxY = 100;
	//	int MinY = -120;

	//	Vector3 mousePos = Input.mousePosition;
	//	Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, mousePos);
	//	Vector2 localPoint;
	//	RectTransformUtility.ScreenPointToLocalPointInRectangle(_skillPopupMenu.GetComponent<RectTransform>(), screenPoint, Camera.main, out localPoint);
	////	localPoint += new Vector2(30.0f, 30.0f);
	//	localPoint += test;
	//	localPoint.x = Mathf.Min(Mathf.Max(localPoint.x, MinX), MaxX);
	//	localPoint.y = Mathf.Min(Mathf.Max(localPoint.y, MinY), MaxY);
		 
	//	_skillPopupMenu.transform.Find("Frame").localPosition = localPoint;
	}

        void OnSkillButton(int skillId, bool isUpgrade)
        {
		if (isUpgrade)
		{
			if (parent[skillId] != -1 && _skillLevels[parent[skillId]] == 0)
				return;

			if (skillPoint == 0)
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


	}

}
