using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
        // Start is called before the first frame update
        [SerializeField] GameObject _firstBG;
        [SerializeField] GameObject _secondBG;
        [SerializeField] GameObject _thirdBG;
        [SerializeField] GameObject _fourthBG;
        [SerializeField] GameObject _bushBG;
        [SerializeField] GameObject _vinesBG;
        [SerializeField]  float _firstBGSpeed = 0.8f;        
        [SerializeField]  float _secondBGSpeed = 0.5f;        
        [SerializeField]  float _thirdBGSpeed = 0.2f;        
        [SerializeField]  float _fourthBGSpeed = 0.0f;        
        [SerializeField]  float _bushBGSpeed = 0.8f;        
        [SerializeField]  float _vinesBGSpeed = 0.0f;         
        

        

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
                UpdateBackgroundPos();
	}

	void UpdateBackgroundPos() 
        {
                float camDir = CameraManager._camHorizonDir;
                 
                Action<GameObject, float> updatePos = (GameObject gm, float speed) => gm.transform.position += new Vector3(camDir * speed * Time.deltaTime, 0.0f, 0.0f);

                updatePos(_firstBG, _firstBGSpeed);
                updatePos(_secondBG, _secondBGSpeed);
                updatePos(_thirdBG, _thirdBGSpeed);
                updatePos(_fourthBG, _fourthBGSpeed);
                updatePos(_bushBG, _bushBGSpeed);
                updatePos(_vinesBG, _vinesBGSpeed);
	}
}
