using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuManager : MonoBehaviour
{
        Canvas canvas;
        Canvas _upgradeMenu;  // prefab
        [SerializeField]Button[] _skillButton = new Button[8];
         
        [SerializeField] Text _skillPoint; 
        Text[] _skillLevelTexts = new Text[8];
        int[] _skillLevels = new int[8];
        bool[] _isActives = new bool[8]; 
         
        int skillPoint = 5;

	private void Awake()
	{ 
		canvas = GetComponent<Canvas>();         
	}
	// Start is called before the first frame update
	void Start()
    { 
                _skillPoint.text = skillPoint.ToString();

		for (int i = 0; i < _skillButton.Length; i++)
                {
                        int k = i;   
                        _skillButton[i].onClick.AddListener(() => {
				ClickButton(k);    
                        });  
                }  
    }  
         
        public void CloseUI()
        {
                gameObject.SetActive(false);

	}
        void ClickButton(int id)
        {
                if (skillPoint == 0)
                {
                        Debug.Log("µ∑ ∫Œ¡∑"); 
                        return;

                }
                if (_isActives[id] == false)
                { 
			_isActives[id] = true;  
                        _skillButton[id].transform.Find("OffMode").gameObject.SetActive(false);
                        Transform gm = _skillButton[id].transform.Find("check");
                        Transform icon = gm.Find("checkIconColor");
			_skillLevelTexts[id] = gm.Find("Frame").Find("SkillLevel").GetComponent<Text>();

			icon.GetComponent<Image>().color = new Color(102.0f / 255.0f, 132 / 255.0f, 1.0f, 1.0f); 

		}
		_skillLevelTexts[id].text = (++_skillLevels[id]).ToString();  

		Debug.Log("Click Button ID : " + id);
                --skillPoint;  
		_skillPoint.text = skillPoint.ToString();

	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
