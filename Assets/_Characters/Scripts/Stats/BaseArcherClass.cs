using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArcherClass : BaseClass {

    public BaseArcherClass()
    {
        Damage = 10;
        Armor = 5;
        Health = 25;
        Mana = 10;

        HealthReg = 0.01f;
        ManaReg = 0.01f;

        CritChance = 0.5f;
        CritDamage = 0.5f;
    }

}
