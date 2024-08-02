using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterGenerator : MonoBehaviour
{
	public GameObject _monsterInstance;
	public int _monsterLimit = 0; 
        public QuestSO _endCondition;
          
	ObjectPool<GameObject> _monsterPool;

        Vector2 _minPos, _maxPos;  
        int _spawnedCnt = 0;


	public void Awake()
	{
                gameObject.GetComponent<SpriteRenderer>().enabled = false; 
                _minPos = transform.position - transform.localScale / 2.0f; 
		_maxPos = transform.position + transform.localScale / 2.0f;

                // Init MonsterPool
                { 
                        _monsterPool = new ObjectPool<GameObject>(
                                createFunc: () =>
                                {
                                        GameObject gm = Instantiate<GameObject>(_monsterInstance);
                                        return gm;
                                },

                                actionOnGet: (obj) =>
                                {
                                        obj.GetComponent<Monster>().InitMonster(GetGenPos());
                                        obj.GetComponent<Monster>()._onDead.AddListener(() =>
                                        {
                                                _monsterPool.Release(obj);
                                        });
                                },

                                actionOnRelease: obj =>
                                {
                                        obj.SetActive(false);
                                        _spawnedCnt--;
                                },

                                collectionCheck: false,
                                defaultCapacity: 20,
                                maxSize: 40
                        );
                }
	}
         
	void Start()
        {
		while (_spawnedCnt  < _monsterLimit)
		{
			StartCoroutine(GenMonster());
			_spawnedCnt++;   
		}  
	} 

        Vector3 GetGenPos()
        { 
                float genX = UnityEngine.Random.Range(_minPos.x, _maxPos.x);
                float genY= UnityEngine.Random.Range(_minPos.y, _maxPos.y);

                return new Vector3(genX, _minPos.y, 0.0f) ;  
        } 
         
	IEnumerator GenMonster(float time = 0.0f)
	{
		yield return new WaitForSeconds(time);
		_monsterPool.Get();  
	}

         
    // Update is called once per frame
        void Update()
        { 
                if (_spawnedCnt <  _monsterLimit / 2)
                {
                        if (_endCondition != null)
                        {
                                _endCondition = QuestManager.instance.UpdateQuestData(_endCondition);
                                if (_endCondition.isClear())
                                {
                                        gameObject.SetActive(false);
                                        return;
                                }
                        }        
                        while (_spawnedCnt < _monsterLimit)
                        {  
				StartCoroutine(GenMonster(UnityEngine.Random.Range(0.1f, 2.0f)));
                                _spawnedCnt++;
			}
		}
        }
}
