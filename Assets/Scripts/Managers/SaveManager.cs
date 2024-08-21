using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
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
        SaveUI _saveUI;

	void Start()
        {
		_inventory = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._inventoryManager;
                _dialogue = UIHandler.instance._dialogue.GetComponent<DialogueManager>();
                _quest = QuestManager.instance;
                _saveUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._saveUI;
		

		LoadAllSaveData();
	}

	public void Load()
	{
		
	}

        public void DeleteSaveFile(string fileName)
        {
                File.Delete(Application.dataPath + "/SaveDatas/" + fileName + ".json"); 
        }

        public void LoadAllSaveData()
        {
		string folderPath = Application.dataPath + "/SaveDatas";
                string[] files = Directory.GetFiles(folderPath);

                Dictionary<long, SaveData> date = new Dictionary<long, SaveData>();

            // 파일 이름들을 출력 (확장자 포함)
                foreach (string file in files)
                {
                        if (file.Substring(file.Length - 5, 5) != ".json")
                                continue;

                    //   Debug.Log(file); 
                        string jsonString = File.ReadAllText(file);
                        Debug.Log(file.Substring(file.Length - 10, 5)); 
			SaveData saveData = JsonUtility.FromJson<SaveData>(jsonString);
                        _savedFiles.Add(file.Substring(file.Length - 10, 5), saveData);

                        long idx = saveData.PlayInfo._date + saveData.PlayInfo._playTime * 60;
     
			date.Add(idx, saveData);
		}

		foreach (var item in date.OrderBy(x => x.Key))
		{
			_saveUI.OnSave(false, item.Value); 
		}
	}
        
        long GetDate(string date)
        {

		long ret = 0;
                int idx = date.Length - 1;

                // yy MM DD hh mm ss
                List<int> arr = new List<int>(); 
                for (int i = idx; i >= 0; i--)
                        if (date[i] >= '0' && date[i] <= '9')
				arr.Add((int)(date[i] - '0'));

                ret += arr[0];
                ret += arr[1] * 10;

		ret += arr[2] * 60;
                ret += arr[3] * 600;

                ret += arr[4] * 3600;
                ret += arr[5] * 36000;

                ret += arr[6] * 3600 * 24;
                ret += arr[7] * 3600 * 240; 

                ret += arr[8] * 3600 * 24 * 31;
                ret += arr[9] * 3600 * 24 * 310;

                ret += (arr[10]-4) * 3600 * 24 * 31 * 12;
                ret += (arr[11]-2) * 3600 * 24 * 31 * 120; 

		return ret;
        }

        public SaveData Save(bool auto)
        {
		SaveData saveData = new SaveData();
                saveData.PlayInfo.scene = SceneManager.GetActiveScene().name;
		saveData.PlayInfo.hp = GameManager.instance.GetPlayer().GetComponent<PlayerController>().hp;
		saveData.PlayInfo._saveDate = DateTime.Now.ToString("yy.MM.dd (HH:mm:ss)");
		if (auto) saveData.PlayInfo._saveDate = "[auto] " + saveData.PlayInfo._saveDate;

		saveData.PlayInfo._playTime = GameManager.instance._playTime;
                saveData.PlayInfo._date = GetDate(DateTime.Now.ToString("yy.MM.dd.HH.mm.ss"));

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
		saveData.fileName = fileName; 
		string path = Application.dataPath + "/SaveDatas/" + fileName +".json";  
		File.WriteAllText(path, jsonString);
                _savedFiles.Add(fileName, saveData);

                return saveData;
	}

        string GenerateStringID()
	{

   //       \, /, :, *, ?, ", <, >, |
		const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
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

        public PlayInfo PlayInfo = new PlayInfo();

        public string fileName;

}


[Serializable]
public class PlayInfo
{
        public string scene;
	public string _saveDate;

        public long _date;
	public int _playTime;
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