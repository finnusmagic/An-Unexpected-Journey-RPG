using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInfo {

    public static void LoadAllInfo()
    {
        GameInfo.PlayerName = PlayerPrefs.GetString("PLAYERNAME");
        GameInfo.PlayerLevel = PlayerPrefs.GetInt("PLAYERLEVEL");
        GameInfo.PlayerModel = PlayerPrefs.GetInt("PLAYERMODEL");

        GameInfo.Damage = PlayerPrefs.GetInt("DAMAGE");
        GameInfo.Armor = PlayerPrefs.GetInt("ARMOR");
        GameInfo.Health = PlayerPrefs.GetInt("HEALTH");
        GameInfo.Mana = PlayerPrefs.GetInt("MANA");

        GameInfo.HealthRegen = PlayerPrefs.GetFloat("HEALTHREGEN");
        GameInfo.ManaRegen = PlayerPrefs.GetFloat("MANAREGEN");

        GameInfo.CritChance = PlayerPrefs.GetFloat("CRITCHANCE");
        GameInfo.CritDamage = PlayerPrefs.GetFloat("CRTIDAMAGE");

        Debug.Log("Info Loaded.");
    }
}
