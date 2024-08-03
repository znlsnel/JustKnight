using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArea : MonoBehaviour
{
        // Start is called before the first frame update
        public GameObject LeftWall;
        public GameObject RightWall;

    void Start()
    {
                LeftWall.GetComponent<SpriteRenderer>().enabled = false;
		RightWall.GetComponent<SpriteRenderer>().enabled = false; 
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
