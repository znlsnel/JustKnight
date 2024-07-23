using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
	// Start is called before the first frame update
	public enum PlayerState : int 
	{
		Idle = 0, 
		Run = 1,
		Roll = 2, 
		Attack = 3,
		IdleBlock = 4,
		Block = 5,
		Jump = 6,
		Fall = 7,
		WallSlider = 8,
		Hurt = 9,
		Death = 10,
	}


	[NonSerialized] public Animator anim;
	[NonSerialized] public PlayerState state = PlayerState.Idle;
	[NonSerialized] public int attackCombo = 1; 
	private string curAnim = "";

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}
	void Start()
	{
        
	}

    // Update is called once per frame
    void Update()
    {
		UpdateAnimation();
    }

	void UpdateAnimation() 
	{ 
		// Death -> Hurt -> Attack -> Fall -> WallSlider -> Jump -> Block -> 
		// TODO
		// if (스킬 사용 중)

		string nextAnim = ""; 
		switch (state)
		{ 
			case PlayerState.Idle: 
				nextAnim = "Idle"; 
				break;
			case PlayerState.Run:
				nextAnim = "Run"; 
				break; 
			case PlayerState.Roll: 
				nextAnim = "Roll";
				break;
			case PlayerState.Attack:
				{
					string combo = (attackCombo).ToString();
					nextAnim = "Attack" + combo; 
				}
				break;
			case PlayerState.IdleBlock:
				nextAnim = "Idle Block";
				break; 
			case PlayerState.Block:
				nextAnim = "Block";
				break;
			case PlayerState.Fall:
				nextAnim = "Fall";
				break; 
			case PlayerState.Jump: 
				nextAnim = "Jump";
				break;
			case PlayerState.WallSlider:
				nextAnim = "Wall Slide"; 
				break;
			case PlayerState.Hurt:
				nextAnim = "Hurt";
				break;
			case PlayerState.Death:
				nextAnim = "Death";
				break;
			default: 
				break; 
		}
		  
		if (nextAnim != "" && curAnim != nextAnim)
		{
			curAnim = nextAnim;
			anim.Play(curAnim); 
		}
	}

}
