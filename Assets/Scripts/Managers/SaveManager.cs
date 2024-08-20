using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
        public Dictionary<string, SaveData> _savedFiles = new Dictionary<string, SaveData>(); 
        public Dictionary<string, QuestSO> _quests = new Dictionary<string, QuestSO>();
        public Dictionary<string, EpisodeSO> _episodes = new Dictionary<string, EpisodeSO>();
        public Dictionary<string, ItemSO> _items = new Dictionary<string, ItemSO>(); 

        Vector3 position; 

        double Time;
        string scene;

        int level;
        string fileName;

        InventoryManager _inventory;
        DialogueManager _dialogue;
        QuestManager _quest;

	void Start()
        {
		_inventory = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._inventoryManager;
                _dialogue = UIHandler.instance._dialogue.GetComponent<DialogueManager>();
                _quest = QuestManager.instance;
	}

	public void Load()
	{
		
	}

        public void LoadAllSaveData()
        {
		string folderPath = Application.dataPath + "/SaveDatas";
                string[] files = Directory.GetFiles(folderPath);

            // 파일 이름들을 출력 (확장자 포함)
                foreach (string file in files)
                {
                        if (!file.Contains(".json"))
                                continue;

                        string jsonString = File.ReadAllText(folderPath + "/" + file);
			SaveData saveData = JsonUtility.FromJson<SaveData>(jsonString);
                        _savedFiles.Add(file.Substring(0, 5), saveData); 
		}
        }
        
        public void Save()
        {
		SaveData saveData = new SaveData();
                saveData.PlayerInfo.scene = SceneManager.GetActiveScene().name;
		saveData.PlayerInfo.hp = GameManager.instance.GetPlayer().GetComponent<PlayerController>().hp;  

		for (int i = 0; i < _inventory._items.Count; i++)
                {
                        ItemSO item = _inventory._items[i];

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
                        itemData.Idx = i; 
			saveData.itemDatas.Add(itemData);		
                }
                foreach (var data in _dialogue._episodes) 
                {
			EpisodeData episode = new EpisodeData();
                        episode.idx = data.Key;
                        episode.state = data.Value._state;  

			saveData.episodes.Add(episode);		
		}
                foreach (var data in _quest._quests)
                {
                        QuestData quest = new QuestData();
                        quest.idx = data.Key;
                        quest.state = data.Value.questState;

                        foreach (QuestTaskSO questTask in data.Value.tasks)
                        {
				TaskData task = new TaskData();
                                task.idx = questTask.name;
                                task.curCnt = questTask.curCnt;
                                quest.taskData.Add(task);
			}
                        saveData.quests.Add(quest);
		}
		string jsonString = JsonUtility.ToJson(saveData, true);

                string fileName = GenerateStringID();

		string path = Application.dataPath + "/SaveDatas/" + fileName +".json"; 
		File.WriteAllText(path, jsonString);
                _savedFiles.Add(fileName, saveData);
	}

        string GenerateStringID()
        {
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*()-=_+,.";
                int len = chars.Length;

                string ret = "";
                for (int i = 0; i < 5; i++)
                {
                        int randIdx = UnityEngine.Random.Range(0, len);
                        ret += chars[randIdx]; 
		}

                if (_savedFiles.ContainsKey(ret))
                        return GenerateStringID();

		return ret;
	}
}



[Serializable]
public class SaveData
{
        public List<ItemData> itemDatas = new List<ItemData>();
        public List<EpisodeData> episodes = new List<EpisodeData>();
        public List<QuestData> quests = new List<QuestData>();

        public PlayerInfo PlayerInfo = new PlayerInfo();

}


[Serializable]
public class PlayerInfo
{
        public string scene;
        public int hp;
}

[Serializable]
public class ItemData
{
        public string Name;
        public int Idx;
        public List<AttributeData> Effects  = new List<AttributeData>();
}

[Serializable]
public class AttributeData
{
	public EPlayerStatus effectType;
	public int value; 
}

[Serializable]
public class EpisodeData
{
        public string idx;
	public EDialogueState state;
}

[Serializable]
public class QuestData
{
	public string idx;
	public EQuestState state;
	public List<TaskData> taskData = new List<TaskData>();
}

[Serializable]
public class TaskData
{
	public string idx;
	public int curCnt;
}