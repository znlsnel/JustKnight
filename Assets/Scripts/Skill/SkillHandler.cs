using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillHandler : Singleton<SkillHandler>
// Start is called before the first frame update
{
	// 능력치들을 넣어주면 될것같음
	// ex ) int 추가공격력 = 10;
	//      ) int  
	//PassiveSkill  
	private PassiveSkillManager psvSkillManager;
	private ActiveSkillManager actSkillManager;

	PassiveSkillManager GetPassiveSkillManager() { return psvSkillManager; }
	ActiveSkillManager GetActiveSkillManager() { return actSkillManager; }
	  
	public override void Awake()
	{
		base.Awake();
		psvSkillManager = gameObject.AddComponent<PassiveSkillManager>();
		actSkillManager = gameObject.AddComponent<ActiveSkillManager>(); 
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
