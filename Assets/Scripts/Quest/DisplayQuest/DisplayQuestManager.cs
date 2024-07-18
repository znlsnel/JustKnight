using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayQuestManager : MonoBehaviour
{
        public List<GameObject> _questSlots = new List<GameObject>();
        public List<QuestSO> _quests = new List<QuestSO>();

        public bool AddQuest(QuestSO quest)
        {
                if (_quests.Count == 3)
                        return false;

                _quests.Add(quest);
                UpdateDisplayQuestSlot();
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
