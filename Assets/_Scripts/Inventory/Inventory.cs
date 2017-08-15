using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory
{
    public class Inventory : MonoBehaviour
    {
        GameObject inventoryPanel;
        GameObject slotPanel;
        ItemDatabase database;
        [Header("Setup")]
        public GameObject inventorySlot;
        public GameObject inventoryItem;

        [Header("Amount of Slots")]
        [Range(0, 100)]
        public int slotAmount = 30;
        public List<Item> items = new List<Item>();
        public List<GameObject> slots = new List<GameObject>();

        private void Start()
        {
            inventoryPanel = GameObject.Find("Inventory Panel");
            slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
            database = GetComponent<ItemDatabase>();

            for (int i = 0; i < slotAmount; i++)
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));
                slots[i].GetComponent<Slot>().slotID = i;
                slots[i].transform.SetParent(slotPanel.transform);
            }

            AddItem(0);
            AddItem(0);
            AddItem(0);
            AddItem(3);
            AddItem(3);
            AddItem(6);
        }

        public void AddItem(int id)
        {
            Item itemToAdd = database.FetchItemByID(id);
            if (itemToAdd.Stackable && ItemAlreadyExists(itemToAdd))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount++;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ID == -1)
                    {
                        items[i] = itemToAdd;
                        GameObject itemObj = Instantiate(inventoryItem);
                        itemObj.GetComponent<ItemData>().item = itemToAdd;
                        itemObj.GetComponent<ItemData>().slotNumber = i;
                        itemObj.transform.SetParent(slots[i].transform);
                        itemObj.transform.position = Vector2.zero;
                        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                        itemObj.name = "Item: " + itemToAdd.Title;
                        break;
                    }
                }
            }
        }

        bool ItemAlreadyExists(Item item)
        {
            for (int i = 0; i < items.Count; i++)
                if (items[i].ID == item.ID)
                    return true;
            return false;
        }
    }
}
