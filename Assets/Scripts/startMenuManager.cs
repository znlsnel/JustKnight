using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startMenuManager : MonoBehaviour
{
       
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }


       public void OnNewGameButton()
        {
                Debug.Log("New Game");
                GameManager.instance.LoadScene("Tutorial");   
        }

	public void OnContinueButton()
        {
		Debug.Log("OnContinueButton");

	}

	public void OnGameQuit()
        {
		Debug.Log("OnGameQuit");

	}
}
