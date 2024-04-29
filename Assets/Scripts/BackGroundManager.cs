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
        [SerializeField] GameObject _player;
        [SerializeField]  float _firstBGSpeed = 0.8f;        
        [SerializeField]  float _secondBGSpeed = 0.5f;        
        [SerializeField]  float _thirdBGSpeed = 0.2f;        
        [SerializeField]  float _fourthBGSpeed = 0.0f;        
        [SerializeField]  float _bushBGSpeed = 0.8f;        
        [SerializeField]  float _vinesBGSpeed = 0.0f;


        PlayerController _playerCtrl;

	private void Awake()
	{
		_playerCtrl = _player.GetComponent<PlayerController>();
		CameraManager._backGroundManager = this; 
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
                        if (gm == null)
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
