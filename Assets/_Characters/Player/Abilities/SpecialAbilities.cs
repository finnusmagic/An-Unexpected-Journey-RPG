using UnityEngine;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] AbilityConfig[] abilities;

        private PlayerStatusManager player;

        void Start()
        {
            player = FindObjectOfType<PlayerStatusManager>();

            AttachInitialAbilities();
            player.UpdatePlayerMana();
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            var manaCost = abilities[abilityIndex].GetManaCost();

            if (manaCost <= player.currentMana)
            {
                player.ConsumeMana(manaCost);
                abilities[abilityIndex].Use(target);
            }
            else
            {
              //  audioSource.PlayOneShot(outOfMana);
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }
    }
}