﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            PlayAbilityAnimation();
            DealDamage(target);
            PlayParticleEffect();
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<EnemyStatsManager>().TakeDamage(damageToDeal);
        }
    }
}