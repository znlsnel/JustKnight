using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestReward", menuName = "Quest/reward/test", order = 1)]

public class TestRewardSO : RewardSO
{
	public string debugLog;
	public override void Get()
	{
		Debug.Log($"Test Reward ! : {debugLog}!");
	}
}
