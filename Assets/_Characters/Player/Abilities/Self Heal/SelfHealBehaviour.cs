using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerStatsManager player = null;

        void Start()
        {
            player = GetComponent<PlayerStatsManager>();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            var playerHealth = player;
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
            PlayAbilityAnimation();
        }
    }
}