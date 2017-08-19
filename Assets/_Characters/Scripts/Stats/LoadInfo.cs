using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInfo {

    public static void LoadAllInfo()
    {
        GameInfo.PlayerName = PlayerPrefs.GetString("PLAYERNAME");
        GameInfo.PlayerLevel = PlayerPrefs.GetInt("PLAYERLEVEL");
        GameInfo.PlayerModel = PlayerPrefs.GetInt("PLAYERMODEL");

        GameInfo.Strength = PlayerPrefs.GetInt("STRENGTH");
        GameInfo.Defense = PlayerPrefs.GetInt("DEFENSE");
        GameInfo.Health = PlayerPrefs.GetInt("HEALTH");
        GameInfo.Mana = PlayerPrefs.GetInt("MANA");

        Debug.Log(GameInfo.Strength);
        Debug.Log("Info Loaded.");
    }
}
