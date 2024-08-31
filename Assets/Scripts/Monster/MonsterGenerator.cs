using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterGenerator : MonoBehaviour
{
	public GameObject _monsterPrefab;
	public int _monsterLimit = 0; 

        public QuestSO _endCondition;
        public QuestSO _startCondition;
          
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
                                        GameObject gm = Instantiate<GameObject>(_monsterPrefab);
                                        return gm;
                                },

                                actionOnGet: (obj) =>
                                { 
                                        obj.GetComponent<Monster>().InitMonster(GetGenPos());
                                        obj.GetComponent<Monster>()._onDestroy = () =>
                                        {
                                                _monsterPool.Release(obj);
                                        }; 
                                },

                                actionOnRelease: obj =>
                                {
                                        if (obj == null)
                                                return;

                                        obj.SetActive(false);
                                        _spawnedCnt--;
                                },

                                collectionCheck: false,
                                defaultCapacity: _monsterLimit, 
                                maxSize: _monsterLimit + 3
			);
                }
	}
         
	void Start()
        {
                if (_startCondition != null)
                        return;

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
                int genCondition = _monsterLimit > 1 ? _monsterLimit / 2 : _monsterLimit; 
                if (_spawnedCnt < genCondition)
                { 
                        if (_endCondition != null)
                        {
                                 QuestManager.instance.UpdateQuestData(ref _endCondition);
                                if (_endCondition.isClear)
                                {
                                        gameObject.SetActive(false);
                                        return;
                                }
                        }

                        if (_startCondition != null && _startCondition.questState != EQuestState.IN_PROGRESS)
                                return;

                        while (_spawnedCnt < _monsterLimit)
                        {  
				StartCoroutine(GenMonster(UnityEngine.Random.Range(0.1f, 2.0f)));
                                _spawnedCnt++;
			}
		}
        }
}
