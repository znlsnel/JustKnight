using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

public class ItemManager : Singleton<ItemManager>
{
        [SerializeField] GameObject _itemObjPrefab;
        private ObjectPool<GameObject> _itemObjPool;
	

	public override void Awake()
	{
		base.Awake();
		
                _itemObjPool = new ObjectPool<GameObject>(
                        createFunc: () => { 
				GameObject gm = Instantiate<GameObject>(_itemObjPrefab);
				DontDestroyOnLoad(gm);
				return gm; 
			},
			actionOnGet: obj => obj.SetActive(true),
			actionOnRelease: obj => obj.SetActive(false),
			actionOnDestroy: Destroy,
			collectionCheck: false,
			defaultCapacity: 10, 
			maxSize: 20
		) ;
	}

	public GameObject GetItemObj(Vector3 genPos)
	{
		GameObject result = _itemObjPool.Get();
		ItemController io = result.GetComponent<ItemController>();
		result.transform.position = genPos;
		io.SpawnItem();
		return result; 
	}

	public void ReleaseObject(GameObject obj)
	{
		_itemObjPool.Release(obj);
	}

	void Start()
        {
                
        }

        // Update is called once per frame
        void Update()
        {
                
        }
}
