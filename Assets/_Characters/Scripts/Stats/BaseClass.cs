using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseClass {

    private int damage;
    private int armor;
    private int health;
    private int mana;

    private float healthReg;
    private float manaReg;

    private float critChance;
    private float critDamage;

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
}
