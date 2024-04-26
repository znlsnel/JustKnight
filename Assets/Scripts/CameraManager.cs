using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
        // Start is called before the first frame update
       public static float _camHorizonDir = 0.0f; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		_camHorizonDir =  Input.GetAxis("Horizontal"); 
                transform.position += new Vector3(_camHorizonDir, 0.0f, 0.0f) * Time.deltaTime; 
    }
}
