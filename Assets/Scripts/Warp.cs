using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
        // Start is called before the first frame update
        public GameObject warpLocation;
	private void Awake()
	{
		GetComponent<SpriteRenderer>().sortingOrder = -1; 
		warpLocation.GetComponent<SpriteRenderer>().sortingOrder = -1; 

	} 
	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerMovementController pm = collision.gameObject.GetComponent<PlayerMovementController>();
		if (pm != null)
		{
			collision.gameObject.transform.position = warpLocation.transform.position;
			pm._rigidbody.velocity = new Vector2 (0, 0); 
		}
	}
}
