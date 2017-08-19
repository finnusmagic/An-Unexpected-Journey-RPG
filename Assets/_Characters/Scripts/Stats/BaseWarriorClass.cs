using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWarriorClass : BaseClass {

    public BaseWarriorClass()
    {
        Damage = 10;
        Armor = 10;
        Health = 100;
        Mana = 5;

        HealthReg = 0.01f;
        ManaReg = 0.01f;

        CritChance = 0.5f;
        CritDamage = 0.5f;
    }

}
