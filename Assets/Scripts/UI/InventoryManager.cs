using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;


public class ItemSlot
{
        public int _id;
}

public class InventoryManager : MonoBehaviour
{
        List<ItemSlot>  slots = new List<ItemSlot>();  

       
    void Start()
    {
        
    }

    void Update()
    {
        
    }

       public  void AddItem(int id)
        {
                ItemSlot s = new ItemSlot();
                s._id = id;
                slots.Add(s); 

                for (int i = 0; i < slots.Count; i++)
                {
                        Debug.Log($"{i} Slot :  {slots[i]._id}");
                }
        }

}
