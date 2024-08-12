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
	bool isMovingToPlayer = false;

	private void Awake() 
	{

		_rigid = gameObject.GetComponent<Rigidbody2D>();
		_inventory = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._inventoryManager;
	}

	private void Update()
	{
		if (isMovingToPlayer)
			MoveToPlayer();
	}


	public void SpawnItem(ItemSO item) 
        {
		_item = item;
		_rigid.simulated = true;

                Vector2 force = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), UnityEngine.Random.Range(1.0f, 2.0f));
		_rigid.AddForce(force, ForceMode2D.Impulse);

		spawnTime = Time.time;
		if (!_inventory.isEmpty())
			Utils.instance.SetTimer(() => { isMovingToPlayer = true; }, 1.0f);
	}  
	 
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isMovingToPlayer || collision.gameObject.layer != LayerMask.NameToLayer("Player"))
			return; 

		if (!_inventory.isEmpty()) 
		{  
			float delay = Math.Clamp(1.0f - (Time.time - spawnTime), 0.0f, 1.0f);  
			Utils.instance.SetTimer(() => { isMovingToPlayer = true; }, delay); 
		}  
	}

	void MoveToPlayer()
	{
		Vector3 dir = (GameManager.instance.GetPlayer().transform.position + new Vector3(0.0f, 0.5f, 0.0f)) - gameObject.transform.position;
		if (dir.magnitude < 0.1f)
		{
			isMovingToPlayer = false;

			if (_inventory.isEmpty())
			{
				_rigid.simulated = true;
				_rigid.velocity = new Vector2(0.1f, _rigid.velocity.y / 2);
				return;
			}

			_rigid.simulated = false;
			_inventory.AddItem(_item);
			ReleaseItem();

			return;
		}

		dir = dir.normalized;
		//gameObject.transform.position += dir * Time.deltaTime * 10.0f;
		_rigid.velocity = dir * 10.0f;
	}

	public void ReleaseItem()
        {
                ItemManager.instance.ReleaseObject(gameObject); 
        }

}
