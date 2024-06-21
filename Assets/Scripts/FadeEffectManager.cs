using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffectManager : MonoBehaviour
{
        // Start is called before the first frame update
        Animator _anim;
	public Action _onFadeOutComplete;
	Canvas _canvas;
	Image _panelImage;

	private void Awake() 
	{
		
		_anim = GetComponent<Animator>();
		_canvas = GetComponentInParent<Canvas>();
		_panelImage = GetComponent<Image>();
	}
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PlayFadeIn()
        {

		_anim.Play("FadeIn"); 

	}
	 
	public void PlayFadeOut()
	{

		_canvas.sortingOrder = 100;
		_anim.Play("FadeOut"); 

 	}
	void AE_EndFadeIn()
	{
		 
		_anim.Play("Empty");  
		_canvas.sortingOrder = -1; 
		//Debug.Log("  EnD IN FADE"); 
	}

	void AE_EndFadeOut()
	{
		if (_onFadeOutComplete != null)
		{
			_onFadeOutComplete.Invoke();
			_onFadeOutComplete = null;
		}
	} 
}
