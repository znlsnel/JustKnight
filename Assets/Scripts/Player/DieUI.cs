using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieUI : MonoBehaviour
{
        [SerializeField] GameObject _startMenu;
        Animator _anim;

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

        public void AE_EndOpenAnim() 
        {
                StartMenu.instance.ActiveStartMenu(true);
                GameManager.instance._onNextScene += () =>
                {
                        GameManager.instance.GetPlayer().GetComponent<PlayerController>()._playerState = EPlayerState.Idle;
                        InputManager.instance.Freezz(false);
                        gameObject.SetActive(false); 
                };
        }

   
}
