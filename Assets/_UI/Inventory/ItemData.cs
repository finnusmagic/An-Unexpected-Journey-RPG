using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.Inventory
{
    public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Item item;
        public int amount;
        public int slotNumber;

        private Transform originalParent;
        private Inventory inv;
        private Tooltip tooltip;
        private Vector2 offset;

        private void Start()
        {
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            tooltip = inv.GetComponent<Tooltip>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item != null)
            {
                offset = eventData.position - (Vector2)this.transform.position;
                this.transform.SetParent(this.transform.parent.parent);
                this.transform.position = eventData.position - offset;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (item != null)
            {
                this.transform.position = eventData.position - offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(inv.slots[slotNumber].transform);
            this.transform.position = inv.slots[slotNumber].transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.Activate(item);
            inv.currentItem = item.ID;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.Deactivate();
        }
    }
}
