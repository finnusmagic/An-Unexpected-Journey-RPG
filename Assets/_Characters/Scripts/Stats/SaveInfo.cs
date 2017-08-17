using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInfo
{

    public static void SaveAllInfo()
    {
        PlayerPrefs.SetString("PLAYERNAME", GameInfo.PlayerName);
        PlayerPrefs.SetInt("PLAYERLEVEL", GameInfo.PlayerLevel);

        PlayerPrefs.SetInt("STRENGTH", GameInfo.Strength);
        PlayerPrefs.SetInt("DEFENSE", GameInfo.Defense);
        PlayerPrefs.SetInt("HEALTH", GameInfo.Health);
        PlayerPrefs.SetInt("MANA", GameInfo.Mana);

        Debug.Log("Info Saved.");
    }

}
