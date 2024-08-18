using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward Item", menuName = "Quest/reward/item", order = 1)]
public class ItemRewardSO : RewardSO
{
	[SerializeField] ItemSO _reward;
	public override string GetReward()
	{
		InventoryManager _inventoryManager = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._inventoryManager;

		ItemSO item = Instantiate(_reward);
		_inventoryManager.AddItem(item);
		 
		return item._name;
	}
}
 