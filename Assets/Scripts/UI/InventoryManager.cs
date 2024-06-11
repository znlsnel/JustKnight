using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType : int
{
	Headband = 0,
	Belt = 1,
	Gloves = 2,
	Shoes = 3,
	// -------------
         
	Amulet = 4,
}
 

public class Item
{
        public Item() { }
	public Item(int type, Text t)
	{
		_itemType = type;
		_name = t;
	}

	public int _itemType; 
        public Text _name;  


} 

public class InventoryManager : MonoBehaviour 
{
        List<Item>  _slots = new List<Item>();
        int _slotTop = 0;
        
	private void Awake()
	{
                Transform slots = transform.Find("slots");
                foreach (Transform child in slots)
                { 
                        if (child.name.Contains("ivnSlot")) 
                        {
                                Text t = child.transform.Find("text")?.GetComponent<Text>();
                             
                                if (t != null)
                                {
					t.text= " - "; 
					_slots.Add(new Item(0, t)); 
				}
                                        
			}
                }

                Debug.Log(_slots.Count);  
	}

	void Start()
        {
                gameObject.SetActive(false);
                
        }

        void Update()
        {
        
        }

        public void ActiveMenu(bool isActive)
        {
                gameObject.SetActive(isActive); 
        }

       public  void AddItem(int id)
        {
                if (_slotTop >= _slots.Count)
                        return;

                _slots[_slotTop]._itemType = id;
                _slots[_slotTop++]._name.text =  id.ToString();
        }

}
