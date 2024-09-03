using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SaveManager : Singleton<SaveManager>
{
        public Dictionary<string, SaveData> _savedFiles = new Dictionary<string, SaveData>(); 

        public Dictionary<string, EpisodeSO> _episodes = new Dictionary<string, EpisodeSO>();
        public Dictionary<string, QuestSO> _quests = new Dictionary<string, QuestSO>();
        public Dictionary<string, ItemSO> _items = new Dictionary<string, ItemSO>(); 

        Vector3 position; 

        double Time;
        string scene;

        int level;
        string fileName;

        InventoryManager _inventory;
        DialogueManager _dialogue;
        DisplayQuest _displayQuest;
        QuestManager _quest;
        QuestUI _questUI;
        SaveUI _saveUI;

	void Start()
        {
		_inventory = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._inventoryManager;
                _dialogue = UIHandler.instance._dialogue.GetComponent<DialogueManager>();
                _quest = QuestManager.instance;
                _saveUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._saveUI;
		_questUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._questUI;
		_displayQuest = UIHandler.instance._displayQuest.GetComponent<DisplayQuest>();


		LoadAllSaveData();
                List<EpisodeSO> episodes = LoadAllAssetsInFolder<EpisodeSO>("Datas/episode");
                List<QuestSO> quests = LoadAllAssetsInFolder<QuestSO>("Datas/episode");
                List<ItemSO> items = LoadAllAssetsInFolder<ItemSO>("Datas/Items");

                foreach (EpisodeSO episode in episodes)
                        _episodes.Add(episode.episodeCode, episode);

                foreach (QuestSO quest in quests)
                        _quests.Add(quest.questCode, quest);

                foreach (ItemSO item in items)
                        _items.Add(item.name, item);
                
	}

	public void Load(SaveData saveData) 
	{
		// scene Load	
		GameManager.instance.LoadScene(saveData.PlayInfo.scene);



		// player Setting
		GameManager.instance._onNextScene += () =>
		{
			Utils.instance.SetTimer(() =>
			{
				GameManager.instance.GetPlayer().transform.position = saveData.PlayInfo.position;
				GameManager.instance.GetPlayer().GetComponent<PlayerController>().hp = saveData.PlayInfo.hp;
			});

			GameManager.instance.ResetGame();

			// Item
			foreach (ItemData itemData in saveData.itemDatas)
			{
				ItemSO item = Instantiate<ItemSO>(_items[itemData.Name]);
				item._effects.Clear();

				foreach (AttributeData data in itemData.Effects)
				{
					item._effects.Add(new SkillAttribute(data.effectType, data.value));
				}
				_inventory.AddItem(item, itemData.Idx);
			}
			// episodes
			foreach (EpisodeData epiData in saveData.episodes)
			{
				EpisodeSO episode = Instantiate<EpisodeSO>(_episodes[epiData.episodeCode]);
				episode._state = epiData.state;
				_dialogue.AddDialogue(episode);
			}
			// quests
			foreach (QuestData questData in saveData.quests)
			{
				QuestSO quest = Instantiate<QuestSO>(_quests[questData.questCode]);
				quest._state = questData.state;
				foreach (TaskData taskData in questData.taskData)
				{
					for (int i = 0; i < quest.tasks.Count; i++)
					{
						if (quest.tasks[i].name == taskData.taskName)
						{
							quest.tasks[i].curCnt = taskData.curCnt;
							break;
						}
					}
				}

				_quest.AddQuest(quest);
				if (quest._state != EQuestState.PENDING)
					_quest.RegisterQuest(quest, questData.isDisplaying);

				_questUI.AddQuest(quest);
			}
		};

		// timer Setting
		GameManager.instance._playTime = saveData.PlayInfo._playTime;
	}

        List<T> LoadAllAssetsInFolder<T>(string folderPath) where T : ScriptableObject
	{
                List<T> assets = new List<T>();
		T[] loadedAssets = Resources.LoadAll<T>(folderPath);
                if (loadedAssets != null)
                        assets.AddRange(loadedAssets);

		return assets; 
        }

	public void DeleteSaveFile(string fileName)
        {
                File.Delete(Application.dataPath + "/SaveDatas/" + fileName + ".json");  
                File.Delete(Application.dataPath + "/SaveDatas/" + fileName + ".json.meta");
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
		int[] arr = date.Where(char.IsDigit).Select(c => c - '0').Reverse().ToArray();

		ret += arr[0] + arr[1] * 10; // 초
		ret += (arr[2] + arr[3] * 10) * 60; // 분
		ret += (arr[4] + arr[5] * 10) * 3600; // 시간
		ret += (arr[6] + arr[7] * 10) * 86400; // 일
		ret += (arr[8] + arr[9] * 10) * 86400 * 31; // 월
		ret += ((arr[10] - 4) + (arr[11] - 2) * 10) * 86400 * 31 * 12; // 년

		return ret;
        }

        public SaveData Save(bool auto, string fileName = "")
        {
		if (fileName == "") 
                        fileName = GenerateStringID(); 

                SaveData saveData =  new SaveData();
		if (_savedFiles.ContainsKey(fileName))
		{
			saveData = _savedFiles[fileName];
			saveData.episodes.Clear();
			saveData.quests.Clear();	
			saveData.itemDatas.Clear();
		}
		 
		saveData.fileName = fileName; 
		saveData.PlayInfo.scene = SceneManager.GetActiveScene().name;
		saveData.PlayInfo.hp = GameManager.instance.GetPlayer().GetComponent<PlayerController>().hp;
		saveData.PlayInfo._saveDate = DateTime.Now.ToString("yy.MM.dd (HH:mm:ss)");
		if (auto) saveData.PlayInfo._saveDate = "[auto] " + saveData.PlayInfo._saveDate;

		saveData.PlayInfo._playTime = GameManager.instance._playTime;
                saveData.PlayInfo._date = GetDate(DateTime.Now.ToString("yy.MM.dd.HH.mm.ss"));
                saveData.PlayInfo.position = GameManager.instance.GetPlayer().transform.position;

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

                foreach (var data in _dialogue.episodes) 
                {
			EpisodeData episode = new EpisodeData();
                        episode.episodeCode = data.Key;
                        episode.state = data.Value._state;  

			saveData.episodes.Add(episode);		
		}

                foreach (var data in _quest._quests)
                {
                        QuestData quest = new QuestData();
                        quest.questCode = data.Key;
                        quest.state = data.Value._state;
			quest.isDisplaying = _displayQuest.IsQuestStored(data.Value);

			foreach (QuestTaskSO questTask in data.Value.tasks)
                        {
				TaskData task = new TaskData();
                                task.taskName = questTask.name;
                                task.curCnt = questTask.curCnt;
                                quest.taskData.Add(task);
			}
                        saveData.quests.Add(quest);
		}

               
		string path = Application.dataPath + "/SaveDatas/" + fileName +".json";  

		string jsonString = JsonUtility.ToJson(saveData, true);
		File.WriteAllText(path, jsonString);

		if (!_savedFiles.ContainsKey(fileName)) 
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
        public Vector3 position; 
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
        public string episodeCode;
	public EEpisodeState state;
}

[Serializable]
public class QuestData
{
	public string questCode;
	public EQuestState state;
	public List<TaskData> taskData = new List<TaskData>();
        public bool isDisplaying;
}

[Serializable]
public class TaskData
{
	public string taskName;
	public int curCnt;
}