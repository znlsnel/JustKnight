using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
        [SerializeField] string nextSceneName; 
    // Start is called before the first frame update
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

	void OnPortal()
	{
		Debug.Log(nextSceneName);  
		GameManager.instance.LoadScene(nextSceneName); 
	} 
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<PlayerController>()._portalAction = null ;
			collision.GetComponent<PlayerController>()._portalAction += OnPortal;  

		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<PlayerController>()._portalAction = null;
		} 
	} 
}
