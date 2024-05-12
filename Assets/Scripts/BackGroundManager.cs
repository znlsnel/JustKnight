using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BackGroundManager : MonoBehaviour
{
        // Start is called before the first frame update
        GameObject _firstBG;
        GameObject _secondBG;
        GameObject _thirdBG;
         GameObject _fourthBG;
       GameObject _bushBG; 
       GameObject _vinesBG;
  
        [SerializeField]  float _firstBGSpeed = 0.8f;        
        [SerializeField]  float _secondBGSpeed = 0.5f;        
        [SerializeField]  float _thirdBGSpeed = 0.2f;        
        [SerializeField]  float _fourthBGSpeed = 0.0f;        
        [SerializeField]  float _bushBGSpeed = 0.8f;        
        [SerializeField]  float _vinesBGSpeed = 0.0f;

	private void Awake()
	{ 
                _firstBG = GameObject.Find("First");
		_secondBG = GameObject.Find("Second");
		_thirdBG = GameObject.Find("Third");
		_fourthBG = GameObject.Find("Fourth");
		_bushBG = GameObject.Find("BACKGROUND_Bush");
		_vinesBG = GameObject.Find("Second_VINES");  

	}
	void Start()
    {

	}

	// Update is called once per frame
	void Update()
    {
        //CameraManager._camHorizonDir
    }
	private void LateUpdate()
	{
                //UpdateBackgroundPos();
	} 

	public void UpdateBackgroundPos(float dx) 
        {
                float camDir = dx; 
                 
                Action<GameObject, float> updatePos = (GameObject gm, float speed) =>
                {
                        if (gm == null ||  true)
                                return;  

                        gm.transform.position += new Vector3(camDir * speed, 0.0f, 0.0f);
                };

                updatePos(_firstBG, _firstBGSpeed);
                updatePos(_secondBG, _secondBGSpeed);
                updatePos(_thirdBG, _thirdBGSpeed);
                updatePos(_fourthBG, _fourthBGSpeed);
                updatePos(_bushBG, _bushBGSpeed);
                updatePos(_vinesBG, _vinesBGSpeed);
	}
}
