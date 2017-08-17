using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePlayerClass {

    private string playerName;
    private int playerLevel;
    private BaseClass playerClass;

    private string className;
    private string classDescription;
    private Image classIcon;

    private int strength;
    private int defense;
    private int health;
    private int mana;

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public int PlayerLevel
    {
        get { return playerLevel; }
        set { playerLevel = value; }
    }

    public BaseClass PlayerClass
    {
        get { return playerClass; }
        set { playerClass = value; }
    }

    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int Mana
    {
        get { return mana; }
        set { mana = value; }
    }

    public string ClassName
    {
        get { return className; }
        set { className = value; }
    }

    public string ClassDescription
    {
        get { return classDescription; }
        set { classDescription = value; }
    }

    public Image ClassIcon
    {
        get { return classIcon; }
        set { classIcon = value; }
    }
}
