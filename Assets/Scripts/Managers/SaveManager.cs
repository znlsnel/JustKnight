using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
        public List<string> _savedFiles = new List<string>();
        public Dictionary<string, QuestSO> _quests = new Dictionary<string, QuestSO>(); 
        List<ItemSO> _items; 
        Vector3 position;

        double Time;
        string scene;

        int level;
        string fileName;

        InventoryManager _inventory;
        QuestManager _questManager;

	void Start()
        {
		_inventory = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._inventoryManager;
                _questManager = QuestManager.instance;
	}


        public void Load()
        {
                
        }

        public void Save()
        {
                string jsonString = ""; 
                foreach (ItemSO item in  _inventory._items)
                {
                        ItemData itemData = new ItemData();
                        itemData.Name = item != null ? item._name : "";
                        if (itemData.Name == "")
                                continue;

                        foreach (SkillAttribute attri in  item._effects)
                        {
				AttributeData attribute = new AttributeData();
				attribute.effectType = attri.effectType;
                                attribute.value = attri.value;

                                itemData.Effects.Add(attribute);
			} 
                        jsonString += System.Text.Json.JsonSerializer.Serialize(itemData, new JsonSerializerOptions { WriteIndented = true });
		}



        }
}

[Serializable]
public class ItemData
{
        public string Name { get; set; }
        public List<AttributeData> Effects { get; set; }
}

[Serializable]
public class AttributeData
{
	public EPlayerStatus effectType;
	public int value; 
}