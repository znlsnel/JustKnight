using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
	[SerializeField] UnityEngine.Object destScene;
	public string _destSceneName { get { return destScene.name; } }
	string curSceneName;

	private void Start()
	{
		curSceneName = SceneManager.GetActiveScene().name;
	} 
	void OnPortal()
	{
		if (destScene == null)
			return;

		GameManager.instance.LoadScene(destScene.name);
		GameManager.instance._onNextScene += () =>
		{
			PortalController[] portals = FindObjectsOfType<PortalController>();

			foreach (var  portal in portals)
			{
				if (portal._destSceneName == curSceneName)
				{
					GameManager.instance.GetPlayer().transform.position = portal.transform.position;
				}
			}

		};
	} 

	void FindPortal()
	{
		
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<PlayerActionController>()._onPortalEntered = null ;
			collision.GetComponent<PlayerActionController>()._onPortalEntered += OnPortal;   

		}
	}
	 
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<PlayerActionController>()._onPortalEntered = null;
		} 
	} 
}
