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

    private int damage;
    private int armor;
    private int health;
    private int mana;

    private float healthReg;
    private float manaReg;

    private float critChance;
    private float critDamage;

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

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public int Armor
    {
        get { return armor; }
        set { armor = value; }
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

    public float HealthReg
    {
        get { return healthReg; }
        set { healthReg = value; }
    }

    public float ManaReg
    {
        get { return manaReg; }
        set { manaReg = value; }
    }

    public float CritChance
    {
        get { return critChance; }
        set { critChance = value; }
    }

    public float CritDamage
    {
        get { return critDamage; }
        set { critDamage = value; }
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
