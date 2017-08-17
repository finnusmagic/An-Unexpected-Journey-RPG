using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseClass {

    private int strength;
    private int defense;
    private int health;
    private int mana;

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
}
