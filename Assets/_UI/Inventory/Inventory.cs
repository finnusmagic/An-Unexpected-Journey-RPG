using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.Inventory
{
    public class Inventory : MonoBehaviour, IPointerExitHandler
    {
        GameObject inventoryPanel;
        GameObject slotPanel;
        ItemDatabase database;
        [Header("Setup")]
        public GameObject inventorySlot = null;
        public GameObject inventoryItem = null;

        [Header("Design Settings")]
        [Range(0, 100)]
        public int slotAmount = 30;
        public List<Item> items = new List<Item>();
        public List<ItemData> itemHolder = new List<ItemData>();
        public List<GameObject> slots = new List<GameObject>();

        [Header("Add / Remove Functions")]
        [SerializeField] Text itemIDText;
        [SerializeField] Button addItemButton;
        int IDNumber = 0;
        public int currentItem = -1;

        private void Start()
        {
            inventoryPanel = GameObject.Find("Inventory Panel");
            slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
            database = GetComponent<ItemDatabase>();

            itemIDText.text = "0";
            addItemButton.onClick.AddListener(AddItemButton);

            for (int i = 0; i < slotAmount; i++)
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));
                slots[i].GetComponent<Slot>().slotID = i;
                slots[i].transform.SetParent(slotPanel.transform);
            }
        }

        public void AddItemButton()
        {
           IDNumber = Convert.ToInt32(itemIDText.text);
           IDNumber = int.Parse(itemIDText.text);
           AddItem(IDNumber);
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
                        itemObj.GetComponent<ItemData>().amount = 1;
                        itemObj.GetComponent<ItemData>().slotNumber = i;
                        itemObj.transform.SetParent(slots[i].transform);
                        itemObj.transform.position = slots[i].transform.position;
                        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                        itemObj.name = "Item: " + itemToAdd.Title;
                        break;
                    }
                }
            }
        }

        public void RemoveItem(int id)
        {
            Item itemToRemove = database.FetchItemByID(id);
            if (itemToRemove.Stackable && ItemAlreadyExists(itemToRemove))
            {
                for (int j = 0; j < items.Count; j++)
                {
                    if (items[j].ID == id)
                    {
                        ItemData data = slots[j].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount--;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                        if (data.amount == 0)
                        {
                            Destroy(slots[j].transform.GetChild(0).gameObject);
                            items[j] = new Item();
                            break;
                        }
                        if (data.amount == 1)
                        {
                            slots[j].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                            break;
                        }
                        break;
                    }
                }
            }
            else for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ID != -1 && items[i].ID == id)
                    {
                        Destroy(slots[i].transform.GetChild(0).gameObject);
                        items[i] = new Item();
                        break;
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

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Cursor left Panel");
        }
    }
}
