using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.Characters;

namespace RPG.Inventory
{
    public class Inventory : MonoBehaviour
    {
        GameObject inventoryPanel;
        GameObject slotPanel;
        ItemDatabase database;

        [Header("Setup")]
        public GameObject inventorySlot = null;
        public GameObject inventoryItem = null;

        [Header("Add Item Panel")]
        [SerializeField] Text itemIDText;
        [SerializeField] Button addItemButton;

        [Header("Equip Item Panel")]
        [SerializeField] GameObject equipItemPanel = null;
        [SerializeField] Button equipItemButton;

        [Header("Drop Item Panel")]
        [SerializeField] GameObject dropItemPanel = null;
        [SerializeField] Button dropItemButton;

        [Header("Delete Item Panel")]
        [SerializeField] GameObject deleteItemPanel = null;
        [SerializeField] Button deleteItemButton;

        [Header("Design Settings")]
        [Range(0, 100)]
        public int slotAmount = 30;
        public List<Item> items = new List<Item>();
        public List<GameObject> slots = new List<GameObject>();

        int IDNumber = 0;
        public int currentItem;
        private Vector3 weaponDrop;
        private WeaponConfig weaponConfig;
        private Weapon weaponToUse;
        int firstTimeCounter = 2;

        private void Start()
        {
            inventoryPanel = GameObject.Find("Inventory Panel");
            weaponConfig = FindObjectOfType<WeaponConfig>();
            slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
            database = GetComponent<ItemDatabase>();
            weaponDrop.z += 1;

            itemIDText.text = "0";
            addItemButton.onClick.AddListener(AddItemButton);

            for (int i = 0; i < slotAmount; i++)
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));
                slots[i].GetComponent<Slot>().slotID = i;
                slots[i].transform.SetParent(slotPanel.transform);
            }

            AddItem(0);
            AddItem(1);
            AddItem(2);
        }

        public void AddItemButton()
        {
           IDNumber = Convert.ToInt32(itemIDText.text);
           IDNumber = int.Parse(itemIDText.text);
           AddItem(IDNumber);
        }

        public void ActivateItemPanels()
        {
            equipItemPanel.SetActive(true);
            dropItemPanel.SetActive(true);
            deleteItemPanel.SetActive(true);

            equipItemButton.onClick.AddListener(EquipItem);
            dropItemButton.onClick.AddListener(DropItem);
            deleteItemButton.onClick.AddListener(DeleteItem);
        }

        public void EquipItem()
        {
            weaponConfig.PutWeaponInHand(database.FetchItemByID(currentItem).Weapon);
            RemoveItem(currentItem);
            equipItemButton.onClick.RemoveListener(EquipItem);
            firstTimeCounter--;

            if (firstTimeCounter <= 0)
            {
                AddItem(weaponConfig.currentWeaponID);
            }
        }

        public void DropItem()
        {
            Instantiate(database.FetchItemByID(currentItem).Weapon, weaponDrop, transform.rotation);
            RemoveItem(currentItem);
            dropItemButton.onClick.RemoveListener(DropItem);
        }

        public void DeleteItem()
        {
            RemoveItem(currentItem);
            deleteItemButton.onClick.RemoveAllListeners();
            deleteItemPanel.SetActive(false);
            deleteItemButton.onClick.RemoveListener(DeleteItem);
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
                        itemObj.AddComponent<Button>().onClick.AddListener(ActivateItemPanels);
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
    }
}
