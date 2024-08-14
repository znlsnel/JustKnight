using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DisplayQuest : MonoBehaviour 
{
        public List<GameObject> _questSlots = new List<GameObject>();
        public List<QuestSO> _quests = new List<QuestSO>();
	Dictionary<QuestSO, GameObject> _questObject = new Dictionary<QuestSO, GameObject>();

	QuestUI _questUI;

	private void Awake()
	{
		_questUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._questUI; 
	}
	public bool IsQuestStored(QuestSO quest)
        {
                foreach (QuestSO questSO in _quests)
                        if (questSO == quest)
                                return true;

                return false;
        }

	public bool AddQuest(QuestSO quest)
        {
                if (_quests.Count == 3)
                        return false;

                _quests.Add(quest);
                UpdateDisplayQuestSlot();

		GameObject gm = Instantiate<GameObject>(_questUI._questSlotPrefab);
		gm.GetComponent<QuestSlotManager>().SetQuestSlot(quest, true);

		_questObject.Add(quest, gm);

		gm.transform.SetParent(_questUI._questParent_displaying.transform);
		gm.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);


		return true;
	}

        public void RemoveQuest(QuestSO quest)
        {
                List<QuestSO> next = new List<QuestSO>();


                for (int i = 0; i < _quests.Count; i++)
                { 
                        if (_quests[i] != quest)
                        {
                                next.Add(_quests[i]);
			}
                }
                _quests = next;
                UpdateDisplayQuestSlot();


                GameObject gm;
		if (_questObject.TryGetValue(quest, out gm))
                {
			_questObject.Remove(quest);
			Destroy(gm); 
                }
	}

        public  void UpdateDisplayQuestSlot()
        {
                for (int i = 0; i < 3; i++)
                {
                        if (_quests.Count > i)
                        { 
                                _questSlots[i].SetActive(true);
                                _questSlots[i].GetComponent<DisplayQuestSlotManager>().SetQuestSlot(_quests[i]);
			}
			else 
                        {
                                _questSlots[i].SetActive(false);
                        } 
                }
        }
}
