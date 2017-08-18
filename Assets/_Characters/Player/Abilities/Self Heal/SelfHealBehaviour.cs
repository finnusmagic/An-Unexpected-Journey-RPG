using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerStatusManager player = null;

        void Start()
        {
            player = GetComponent<PlayerStatusManager>();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            var playerHealth = player;
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
        }
    }
}