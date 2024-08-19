using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieUI : MonoBehaviour
{
        Animator _anim;

        GameObject player;
        GameObject _player 
        {
                get
                {
                        if (player == null)
                                player = GameManager.instance.GetPlayer();
                        return player;
                }
        }
	PlayerController _playerCtrl
        {
                get 
                { 
                        if (_player == null)
                                return null;
                        return _player.GetComponent<PlayerController>();
                }
        }

    // Start is called before the first frame update
        void Start()
        {
		_anim = gameObject.GetComponent<Animator>();
                gameObject.SetActive(false);
        }

        public void Open()
        {
                InputManager.instance.Freezz(true);  
                gameObject.SetActive(true);
                _anim.Play("open"); 
        }


   
}
