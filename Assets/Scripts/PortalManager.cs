using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;


public class PortalManager : MonoBehaviour
{
	[SerializeField] UnityEngine.Object _destScene;
	
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
		if (_destScene == null)
			return;

		GameManager.instance.LoadScene(_destScene.name); 
	} 
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<PlayerController>()._onPortalEntered = null ;
			collision.GetComponent<PlayerController>()._onPortalEntered += OnPortal;  

		}
	}
	 
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<PlayerController>()._onPortalEntered = null;
		} 
	} 
}
