using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestReward", menuName = "Quest/reward/test", order = 1)]

public class TestRewardSO : RewardSO
{
	public override string GetReward()
	{
		int item = Random.Range(0, 999);
		UIHandler.instance._inventoryManager.AddItem(item);

		return $"æ∆¿Ã≈€ [ {item} ] »πµÊ!";
	}
}
