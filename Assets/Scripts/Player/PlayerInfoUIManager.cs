using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUIManager : MonoBehaviour
{
        // Start is called before the first frame update 
        public Material _hpMaterial;

	private void Awake()  
	{
                //_hpMaterial = GameObject.Find("UserInfoUI").GetComponentInChildren<Material>;
                GameObject gm = GameObject.Find("M_HpBar"); 
                if (gm != null)
                {
                        _hpMaterial = gm.GetComponent<Image>().material;
                }

        }

	public void UpdateHpBar(int maxHp, int curHp)
        { 
                if (_hpMaterial == null)
                {
                        Debug.Log(" hp material isn't vaild ");
                        return;
                }
                 
                _hpMaterial.SetFloat("_HpPercent", (float)curHp / (float)maxHp);  
        }
} 
 