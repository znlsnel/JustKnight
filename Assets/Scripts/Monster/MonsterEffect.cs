using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffect : MonoBehaviour
{
        Animator _anim;

        void Start()
        {
                _anim = GetComponent<Animator>();
        } 
         
        public void PlayAnim(string anim, bool playerCount = false)
        {
                _anim.Play(anim); 
                 
                if(playerCount)
                {
                        Vector3 scale = gameObject.transform.localScale;
                        float d = GameManager.instance.GetPlayer().transform.position.x - gameObject.transform.position.x;
                        int dir = d > 0 ? -1 : 1;

                        gameObject.transform.localScale = new Vector3(Mathf.Abs(scale.x) * dir, scale.y, scale.z);


		}
        }
}
