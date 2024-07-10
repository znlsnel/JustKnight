using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RewardSO : ScriptableObject
{
	public string _description; 
	public abstract string GetReward(); 
}
