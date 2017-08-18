using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class PlayerStatusManager : MonoBehaviour
    {

        public GameObject playerStatus;

        Text hpText = null;
        Text manaText = null;
        Image hpImage;
        Image manaImage;

        [HideInInspector]
        public float currentHealth;
        [HideInInspector]
        public float maxHealth;

        [HideInInspector]
        public float currentMana;
        [HideInInspector]
        public float maxMana;

        [HideInInspector]
        public float currentDamage = 0;
        [HideInInspector]
        public float maxDamage = 0;

        [HideInInspector]
        public float currentArmor = 0;
        [HideInInspector]
        public float maxArmor = 0;

        [HideInInspector]
        public float manaRegenPointsPerSecond = 1f;
        [HideInInspector]
        public float healthRegenPointsPerSecond = 1f;

        private void Start()
        {
            if (playerStatus != null)
            {
                hpText = playerStatus.transform.GetChild(0).GetChild(3).GetComponent<Text>();
                hpImage = playerStatus.transform.GetChild(0).GetChild(1).GetComponent<Image>();

                manaText = playerStatus.transform.GetChild(1).GetChild(3).GetComponent<Text>();
                manaImage = playerStatus.transform.GetChild(1).GetChild(1).GetComponent<Image>();

                maxHealth = GameInfo.Health;
                maxMana = GameInfo.Mana;

                currentHealth = maxHealth;
                currentMana = maxMana;

                UpdatePlayerHealth();
                UpdatePlayerMana();
            }
        }

        private void Update()
        {
            if (currentHealth < maxHealth)
            {
                AddHealthPoints();
                UpdatePlayerHealth();
            }

            if (currentMana < maxMana)
            {
                AddManaPoints();
                UpdatePlayerMana();
            }
        }

        public void DamagePlayer(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
        }

        public void ConsumeMana(float amount)
        {
            currentMana = Mathf.Clamp(currentMana - amount, 0f, maxMana);
        }

        public void UpdatePlayerHealth()
        {
            hpText.text = Convert.ToInt32(currentHealth) + " / " + Convert.ToInt32(maxHealth);
            float fillAmount = currentHealth / maxHealth;
            hpImage.fillAmount = fillAmount;

            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public void UpdatePlayerMana()
        {
            manaText.text = Convert.ToInt32(currentMana) + " / " + Convert.ToInt32(maxMana);
            float fillAmount = currentMana / maxMana;
            manaImage.fillAmount = fillAmount;

            if (currentMana >= maxMana)
            {
                currentMana = maxMana;
            }
        }

        public void AddManaPoints()
        {
            var pointsToAdd = manaRegenPointsPerSecond * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana + pointsToAdd, 0, maxMana);
        }

        public void AddHealthPoints()
        {
            var pointsToAdd = healthRegenPointsPerSecond * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth + pointsToAdd, 0, maxHealth);
        }

        public void Heal(float points)
        {
            currentHealth = Mathf.Clamp(currentHealth + points, 0f, maxHealth);
        }
    }
}
