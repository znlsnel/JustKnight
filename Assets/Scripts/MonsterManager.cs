using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
        // Start is called before the first frame update
        [SerializeField] GameObject _skeletalPrefab;
        List<GameObject> _skeletals = new List<GameObject>();
         
        void InitMonsterManager()
        {
                GameObject[] generator = GameObject.FindGameObjectsWithTag("MonsterGenerator");

		List<GameObject> skeletals = new List<GameObject>();
		for (int i = 0; i < generator.Length; i++)
                {
			if (i < _skeletals.Count)
                        {
                                _skeletals[i].SetActive(true);
                                _skeletals[i].GetComponent<MonsterController>().InitMonster();
                                _skeletals[i].transform.position = generator[i].transform.position;
			}
                        else 
                        {  
				skeletals.Add(Instantiate(_skeletalPrefab));
				skeletals[skeletals.Count - 1].GetComponent<MonsterController>().InitMonster();
                                skeletals[skeletals.Count - 1].transform.position = generator[i].transform.position;
                        } 
                        generator[i].SetActive(false); 
		} 
                  
                foreach(var i in skeletals)
                {
                        _skeletals.Add(i);
                }

        }
    void Start()
    {
                InitMonsterManager(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
