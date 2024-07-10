using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSuccessUIManager : MonoBehaviour
{
        [SerializeField] public Text _description;
	// Start is called before the first frame update
	private void Awake()
	{
		gameObject.SetActive(false);
	}
	public void OpenSuccessUI(string rewardDecription)
        { 
                gameObject.SetActive(true);
                _description.text = "퀘스트 성공!" + "\n" + $"보상 :  {rewardDecription} "; 
        }

        public void OnSuccessUIButton()
        {
                gameObject.SetActive(false);
        }
}
