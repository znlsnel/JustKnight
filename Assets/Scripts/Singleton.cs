using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton< T > : MonoBehaviour where T : Component
{
        // Start is called before the first frame update
        private static T _instance;
        
        
        public static T instance
        {
                get
                {
                        if (_instance == null)
                        {
                                _instance = FindObjectOfType<T>();

                                if (_instance == null)
                                {
                                        GameObject obj = new GameObject();
                                        obj.name = typeof(T).Name;
                                        _instance = obj.AddComponent<T>();
                                }
                        }
                         
                        return _instance;
                }
                
        }
	public  virtual void Awake()
	{
		if (_instance == null)
                {
                        _instance = this as T;
                        DontDestroyOnLoad(gameObject);
                }
                else
                {
                        Destroy(gameObject);
                }
	}

	void Start()
	{
		
	}

	 void Update()
	{
		
	}
}
