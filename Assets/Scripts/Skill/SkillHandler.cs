using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillHandler : Singleton<SkillHandler>
// Start is called before the first frame update
{
	// �ɷ�ġ���� �־��ָ� �ɰͰ���
	// ex ) int �߰����ݷ� = 10;
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
