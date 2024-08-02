using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fps : MonoBehaviour
{
        // Start is called before the first frame update
        public Text fpsText;
    void Start()
    {
        StartCoroutine(FPS());
    }

    // Update is called once per frame
    void Update() 
    {
    }
        
        IEnumerator FPS()
        {
               while (true)
                {
                        yield return new WaitForSeconds(1.0f);
                        int curFps = (int)(1.0f / Time.deltaTime); 
			fpsText.text = curFps.ToString(); 
		}
	}
}
