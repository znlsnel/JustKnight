using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
        // Start is called before the first frame update
        Rigidbody2D _rigid;

	private void Awake()
	{
                _rigid = gameObject.GetComponent<Rigidbody2D>();
                  
	}
	        void Start()
            { 
		
            }

            // Update is called once per frame
            void Update()
            {
        
            }

        public void SpawnItem() 
        {   
                     
                Vector2 force = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, 2.0f));
                _rigid.AddForce(force, ForceMode2D.Impulse);
        } 

        public void ReleaseItem()
        {
                ItemManager.instance.ReleaseObject(gameObject); 
        }
}
