using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory
{
    public class Tooltip : MonoBehaviour
    {
        Item item;
        private string data;
        private GameObject tooltip;

        private void Start()
        {
            tooltip = GameObject.Find("Tooltip");
            tooltip.SetActive(false);
        }

        private void Update()
        {
            if(tooltip.activeSelf)
            {
                tooltip.transform.position = Input.mousePosition;
            }
        }

        public void Activate(Item item)
        {
            this.item = item;
            ConstructDataString();
            tooltip.SetActive(true);
        }

        public void Deactivate()
        {
            tooltip.SetActive(false);
        }

        public void ConstructDataString()
        {
            data = "<color=#0473f0><b>" + item.Title + "</b></color>\n\n" +  
                item.Description + "\n\n" + 
                "Power: " + item.Power + "\n" + 
                "Defence: " + item.Defence + "\n" + 
                "Vitality: " + item.Vitality + "\n" + 
                "Value: " + item.Value;

            tooltip.transform.GetChild(0).GetComponent<Text>().text = data;

        }
    }
}
