using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class UI_Toggle : MonoBehaviour
    {
        [SerializeField]
        GameObject inventoryPanel;
        bool toggleInventory = false;

        private void Update()
        {
            if (toggleInventory)
            {
                inventoryPanel.SetActive(true);
            }
            else
            {
                inventoryPanel.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                toggleInventory = !toggleInventory;
            }
        }
    }
}
