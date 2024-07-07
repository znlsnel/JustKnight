using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public class MonsterMapping
{
	public string monsterName;
	public GameObject instance;
}

public class MonsterGenerator : Singleton<MonsterGenerator>
{
        // Start is called before the first frame update
	[SerializeField] List<MonsterMapping> _monsterMappings;
	private Dictionary<string, GameObject> _monsterDictionary = new Dictionary<string, GameObject>();

        [SerializeField] GameObject _monsterInstance;
         
	private List<Vector3> _spawnPoints = new List<Vector3>();
        private List<GameObject> _activeObjects = new List<GameObject>();
	private ObjectPool<GameObject> _monsterPool;

	[SerializeField] int _maxMonsterSpawnCnt = 10;
        int _spawnCnt = 0;



	public override void Awake()
	{
                foreach (var monster in _monsterMappings)
			_monsterDictionary.Add(monster.monsterName, monster.instance);

		 

                base.Awake(); 
                _monsterPool = new ObjectPool<GameObject>(
                        createFunc: () => 
                        {
                                GameObject gm = Instantiate<GameObject>(_monsterInstance);
                                _activeObjects.Add(gm);
				DontDestroyOnLoad(gm);
                                return gm; 
                        },

                        actionOnGet: (obj) => 
                        { 
                                obj.GetComponent<Monster>().InitMonster(GetGenPos());
                                obj.GetComponent<Monster>()._onDead .AddListener(() => {
                                        _monsterPool.Release(obj); 
                                }); 
                        },

                        actionOnRelease: obj => { 
                                obj.SetActive(false); 
                                _spawnCnt--; 
                        },

                        collectionCheck: false,          
                        defaultCapacity: 20,
                        maxSize: 40 
                ) ;
	} 

	void InitMonsterManager()
        {
		foreach (GameObject obj in _activeObjects)
                        _monsterPool.Release(obj);

                _spawnPoints.Clear();


		GameObject[] generators = GameObject.FindGameObjectsWithTag("MonsterGenerator");
		foreach  (var generator in generators)
                {
			_spawnPoints.Add(generator.transform.position);
                        generator.SetActive(false); 
		} 

                _spawnCnt = 0;
		for (int i = 0; _spawnPoints.Count > 0 & i < _maxMonsterSpawnCnt; i++)
		{
			StartCoroutine(InitMonsterRegister(UnityEngine.Random.Range(0.1f, 2.0f)));
                        Debug.Log("코루틴 외않돼");
			_spawnCnt++; 
		} 
	}

        Vector3 GetGenPos()
        { 
                int idx = UnityEngine.Random.Range(0, _spawnPoints.Count );
                return _spawnPoints[idx];
        } 

	IEnumerator InitMonsterRegister(float time)
	{
		yield return new WaitForSeconds(time);
	        if (_spawnPoints.Count > 0)
			_monsterPool.Get();   
	}

	

	void Start()
        {
                GameManager.instance._onSceneInit += InitMonsterManager;
		InitMonsterManager();
	} 

         

    // Update is called once per frame
        void Update()
        { 
                if (_spawnPoints.Count > 0 &&  _spawnCnt <  _maxMonsterSpawnCnt / 2)
                {
                        for (int i = 0; i < _maxMonsterSpawnCnt  - _spawnCnt; i++)
                        {  
				StartCoroutine(InitMonsterRegister(UnityEngine.Random.Range(0.1f, 2.0f)));
			}
                        _spawnCnt = _maxMonsterSpawnCnt;

		}
        }
}
