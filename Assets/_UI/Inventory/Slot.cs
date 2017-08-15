using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.Inventory
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        public int slotID;
        private Inventory inv;

        private void Start()
        {
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

            if ( inv.items[slotID].ID == -1)
            {
                inv.items[droppedItem.slotNumber] = new Item();
                inv.items[slotID] = droppedItem.item;
                droppedItem.slotNumber = slotID;
            }

            else if (droppedItem.slotNumber != slotID)
            {
                Transform itemInSlot = this.transform.GetChild(0);
                itemInSlot.GetComponent<ItemData>().slotNumber = droppedItem.slotNumber;
                itemInSlot.transform.SetParent(inv.slots[droppedItem.slotNumber].transform);
                itemInSlot.transform.position = inv.slots[droppedItem.slotNumber].transform.position;

                droppedItem.slotNumber = slotID;
                droppedItem.transform.SetParent(this.transform);
                droppedItem.transform.position = this.transform.position;

                inv.items[droppedItem.slotNumber] = itemInSlot.GetComponent<ItemData>().item;
                inv.items[slotID] = droppedItem.item;
            }
        }
    }
}
