using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
        // Start is called before the first frame update  
        public Material _hpMaterial;

	private void Awake()  
	{
                gameObject.SetActive(false); 
        }
         
	public void UpdateHpBar(int curHp, int maxHp)
        { 
                if (_hpMaterial == null)
                {
                        Debug.Log(" hp material isn't vaild ");
                        return;
                }
                 
                _hpMaterial.SetFloat("_HpPercent", (float)curHp / (float)maxHp);  
        }
} 
 