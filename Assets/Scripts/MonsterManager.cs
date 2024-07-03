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
                                MonsterController mc = _skeletals[i].GetComponent<MonsterController>();
                                StartCoroutine(InitMonsterRegister(mc, generator[i].transform.position, UnityEngine.Random.Range(0.1f, 2.0f)));
			}
                        else 
                        {  
				skeletals.Add(Instantiate(_skeletalPrefab));
    
				MonsterController mc = skeletals[skeletals.Count - 1].GetComponent<MonsterController>();
				StartCoroutine(InitMonsterRegister(mc, generator[i].transform.position, UnityEngine.Random.Range(0.1f, 2.0f)));
                        } 
                        generator[i].SetActive(false); 
		}    

                                 
                foreach(var i in skeletals) { 
                        _skeletals.Add(i);
                }

        }
	IEnumerator InitMonsterRegister(MonsterController mc, Vector3 pos, float time)
	{
		yield return new WaitForSeconds(time);
                mc.InitMonster(pos);
               
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
