using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemQuestStep : QuestStep
{
        [SerializeField] GameObject item;
        [SerializeField] int targetItemCount; 

        private int itemCount = 0; 
        public void SetItemCount(int cnt)
        {
                itemCount = cnt;
                if (itemCount > targetItemCount) {
                        FinishQestStep();
                }
        }

}
