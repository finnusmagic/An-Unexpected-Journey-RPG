using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Database;

public class LootScript : MonoBehaviour {

    [System.Serializable]
	public class DropItem
    {
        public GameObject itemToDrop;
        public int dropRarity;
    }

    public List<DropItem> LootTable = new List<DropItem>();
    public int dropChance;

    public void CalculateLoot()
    {
        int calc_DropChance = UnityEngine.Random.Range(0, 101);

        if (calc_DropChance > dropChance)
        {
            Debug.Log("No Loot dropped.");
            return;
        }
        if (calc_DropChance <= dropChance)
        {
            int itemWeight = 0;

            for (int i = 0; i < LootTable.Count; i++)
            {
                itemWeight += LootTable[i].dropRarity;
            }
            Debug.Log("ItemWeight= " + itemWeight);

            int randomValue = UnityEngine.Random.Range(0, itemWeight);

            for (int j = 0; j < LootTable.Count; j++)
            {
                if (randomValue <= LootTable[j].dropRarity)
                {
                    Instantiate(LootTable[j].itemToDrop, transform.position, Quaternion.identity);
                    return;
                }
                randomValue -= LootTable[j].dropRarity;
                Debug.Log("Random value decreased " + randomValue);
            }
        }
    }
}
