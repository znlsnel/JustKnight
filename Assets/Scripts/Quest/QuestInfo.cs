using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQuestState
{
        REQUIREMENTS_NOT_MET,
        CAN_START,
        IN_PROGRESS,
        CAN_FINISH,
        FINISHED,
}

public enum EQuestStepType
{ 
        HUNT_MONSTER,
        COLLECT_ITEM,
        FIND_NPC
}

public class QuestInfo : MonoBehaviour
{
        public QuestSO _info;
        public EQuestState _state;
        public List<EQuestStepType> _questTypes = new List<EQuestStepType>();

        private void Awake()
        {
                /*   foreach (GameObject stp in _info.questSteps)
                   {
                           if (stp.GetComponent<CollectItemQuestStep>() != null)
                                   _questTypes.Add(EQuestStepType.COLLECT_ITEM);
                           else if (stp.GetComponent<HuntMonsterQuestStep>() != null)
                                   _questTypes.Add(EQuestStepType.HUNT_MONSTER); 
                   }
            */
        }
}
