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
                _description.text = "����Ʈ ����!";
                if (rewardDecription != "")
			_description.text += "\n" + $"���� :  {rewardDecription} "; 
                StartCoroutine(CloseUI()); 
        }
         
	IEnumerator CloseUI()
        {
                yield return new WaitForSeconds(1.5f);
                OnSuccessUIButton();
	}

	public void OnSuccessUIButton()
        {
                gameObject.SetActive(false);
        }
}
