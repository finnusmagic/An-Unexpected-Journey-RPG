using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInfo
{

    public static void SaveAllInfo()
    {
        PlayerPrefs.SetString("PLAYERNAME", GameInfo.PlayerName);
        PlayerPrefs.SetInt("PLAYERLEVEL", GameInfo.PlayerLevel);
        PlayerPrefs.SetInt("PLAYERMODEL", GameInfo.PlayerModel);

        PlayerPrefs.SetInt("DAMAGE", GameInfo.Damage);
        PlayerPrefs.SetInt("ARMOR", GameInfo.Armor);
        PlayerPrefs.SetInt("HEALTH", GameInfo.Health);
        PlayerPrefs.SetInt("MANA", GameInfo.Mana);

        PlayerPrefs.SetFloat("HEALTHREGEN", GameInfo.HealthRegen);
        PlayerPrefs.SetFloat("MANAREGEN", GameInfo.ManaRegen);

        PlayerPrefs.SetFloat("CRITCHANCE", GameInfo.CritChance);
        PlayerPrefs.SetFloat("CRITDAMAGE", GameInfo.CritDamage);

        Debug.Log("Info Saved.");
    }

}
