using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemController : MonoBehaviour
{
	// Start is called before the first frame update
	InventoryManager _inventory;
        Rigidbody2D _rigid;
	ItemSO _item;

	float spawnTime = 0;
	bool _isPicked = false;

	private void Start() 
	{
                _rigid = gameObject.GetComponent<Rigidbody2D>();
		_inventory = UIHandler.instance._inventory.GetComponent<InventoryManager>();
	}
	 
	private void Update()
	{
		if (_isPicked)
			MoveToPlayer();
		else
			CheckPlayerColliding(); 
	}
	void MoveToPlayer()
	{
		Vector3 dir = (GameManager.instance.GetPlayer().transform.position + new Vector3(0.0f, 0.5f, 0.0f)) - gameObject.transform.position; 
		if (dir.magnitude < 0.1f)
		{
			_isPicked = false;
			_inventory.AddItem(_item);
			ReleaseItem();
			 
			return;
		}	
		
		_rigid.simulated = false;
		dir = dir.normalized;
		gameObject.transform.position += dir * Time.deltaTime * 10.0f; 
	}

	public void SpawnItem(ItemSO item) 
        {
		if (item == null)
		{
			Debug.Log("Çæ·©¹æ±¸");
		}
		_item = item;
		_rigid.simulated = true;

                Vector2 force = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), UnityEngine.Random.Range(1.0f, 2.0f));
		_rigid.AddForce(force, ForceMode2D.Impulse);

		spawnTime = Time.time;
	}  

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
			return; 

		float dist = (collision.transform.position - gameObject.transform.position).magnitude;
		if (!_inventory.isEmpty() && dist < 1.0f) 
		{  
			float delay = Math.Clamp(1.0f - (Time.time - spawnTime), 0.0f, 1.0f);  
			StartCoroutine(ReleaseAfterTime(delay)); 
		}  
	} 

	void CheckPlayerColliding()
	{
		if (true || Time.time - spawnTime < 1.0f)
			return;

		float dist = (GameManager.instance.GetPlayer().transform.position - gameObject.transform.position).magnitude;
		if (dist < 1.5f)
		{
			_isPicked = true; 
		}
	}

	public void ReleaseItem()
        {
                ItemManager.instance.ReleaseObject(gameObject); 
        }

	IEnumerator ReleaseAfterTime(float delay)
	{
		yield return new WaitForSeconds(delay);
		_isPicked = true;
	}

}
