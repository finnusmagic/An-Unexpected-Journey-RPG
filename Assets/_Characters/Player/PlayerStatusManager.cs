using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Database;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG.Characters
{
    public class PlayerStatusManager : MonoBehaviour
    {
        [Header("Panel Setup")]
        public GameObject playerStatusPanel;
        public GameObject playerStatsPanel;
        public GameObject playerAbilityPanel;

        private Character character;

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
        public float currentHealthReg = 0;
        [HideInInspector]
        public float maxHealthReg = 0;
        [HideInInspector]
        public float currentManaReg = 0;
        [HideInInspector]
        public float maxManaReg = 0;

        [HideInInspector]
        public float currentCritChance = 0;
        [HideInInspector]
        public float maxCritChance = 0;
        [HideInInspector]
        public float currentCritDamage = 0;
        [HideInInspector]
        public float maxCritDamage = 0;

        float criticalHitChance;

        Text Damage;
        Text Armor;
        Text Health;
        Text Mana;

        Text HealthReg;
        Text ManaReg;

        Text CritChance;
        Text CritDamage;

        private LevelUpSystem playerLevel;
        private PlayerInventoryManager playerInventory;


        private void Start()
        {
            playerLevel = FindObjectOfType<LevelUpSystem>();
            playerInventory = FindObjectOfType<PlayerInventoryManager>();

            if (playerStatusPanel != null)
            {
                hpText = playerStatusPanel.transform.GetChild(0).GetChild(3).GetComponent<Text>();
                hpImage = playerStatusPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>();

                manaText = playerStatusPanel.transform.GetChild(1).GetChild(3).GetComponent<Text>();
                manaImage = playerStatusPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>();

                InitiatePlayerStats();
            }
        }

        private void Update()
        {
            if (currentHealth < maxHealth)
            {
                AddHealthPoints();
            }

            if (currentMana < maxMana)
            {
                AddManaPoints();
            }

            UpdatePlayerStats();
        }

        public float CalculateDamage()
        {
            float damageBeforeCritical = GameInfo.Damage + playerInventory.itemDamage + playerLevel.levelDamage;
            float TotalCritDamage = damageBeforeCritical * (GameInfo.CritDamage + playerInventory.itemCritDamage + playerLevel.levelCritDamage) / 100;
            float TotalCritChance = (GameInfo.CritChance + playerInventory.itemCritChance + playerLevel.levelCritChance);
            bool isCriticalHit = UnityEngine.Random.Range(0f, 100f) <= TotalCritChance;

            if(isCriticalHit)
            {
                return damageBeforeCritical + TotalCritDamage;
            }
            else
            {
                return damageBeforeCritical;
            }
        }

        void RestartScene()
        {
            SceneManager.LoadScene("02_Start_Game_Scene");
        }

        public void InitiatePlayerStats()
        {
            maxDamage = GameInfo.Damage;
            maxArmor = GameInfo.Armor;
            maxHealth = GameInfo.Health;
            maxMana = GameInfo.Mana;

            maxHealthReg = GameInfo.HealthRegen;
            maxManaReg = GameInfo.ManaRegen;

            maxCritChance = GameInfo.CritChance;
            maxCritDamage = GameInfo.CritDamage;

            Damage = playerStatsPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>();
            Armor = playerStatsPanel.transform.GetChild(2).GetChild(1).GetComponent<Text>();
            Health = playerStatsPanel.transform.GetChild(2).GetChild(2).GetComponent<Text>();
            Mana = playerStatsPanel.transform.GetChild(2).GetChild(3).GetComponent<Text>();

            HealthReg = playerStatsPanel.transform.GetChild(4).GetChild(2).GetComponent<Text>();
            ManaReg = playerStatsPanel.transform.GetChild(4).GetChild(3).GetComponent<Text>();

            CritChance = playerStatsPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>();
            CritDamage = playerStatsPanel.transform.GetChild(4).GetChild(1).GetComponent<Text>();

            Damage.text = maxDamage.ToString();
            Armor.text = maxArmor.ToString();
            Health.text = maxHealth.ToString();
            Mana.text = maxMana.ToString();

            HealthReg.text = maxHealthReg.ToString();
            ManaReg.text = maxManaReg.ToString();

            CritChance.text = maxCritChance.ToString();
            CritDamage.text = maxCritDamage.ToString();

            currentDamage = maxDamage;
            currentArmor = maxArmor;
            currentHealth = maxHealth;
            currentMana = maxMana;

            currentHealthReg = maxHealthReg;
            currentManaReg = maxManaReg;

            currentCritChance = maxCritChance;
            currentCritDamage = maxCritDamage;
        }

        public void UpdatePlayerStats()
        {
            maxDamage = GameInfo.Damage + playerInventory.itemDamage + playerLevel.levelDamage;
            maxArmor = GameInfo.Armor + playerInventory.itemArmor + playerLevel.levelArmor;
            maxHealth = GameInfo.Health + playerInventory.itemHealth + playerLevel.levelHealth;
            maxMana = GameInfo.Mana + playerInventory.itemMana + playerLevel.levelMana;

            maxHealthReg = GameInfo.HealthRegen + playerInventory.itemHealthReg + playerLevel.levelHealthReg;
            maxManaReg = GameInfo.ManaRegen + playerInventory.itemManaReg + playerLevel.levelManaReg;

            maxCritChance = GameInfo.CritChance + playerInventory.itemCritChance + playerLevel.levelCritChance;
            maxCritDamage = GameInfo.CritDamage + playerInventory.itemCritDamage + playerLevel.levelCritDamage;

            Damage.text = maxDamage.ToString();
            Armor.text = maxArmor.ToString();
            Health.text = maxHealth.ToString();
            Mana.text = maxMana.ToString();

            HealthReg.text = maxHealthReg.ToString() + " %";
            ManaReg.text = maxManaReg.ToString() + " %";

            CritChance.text = maxCritChance.ToString() + " %";
            CritDamage.text = maxCritDamage.ToString() + " %";

            UpdatePlayerMana();
            UpdatePlayerHealth();
        }

        public void DamagePlayer(float damage)
        {
            character = GetComponent<Character>();
            character.CreateFloatingText(damage.ToString(), transform);
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);

            bool characterDies = (currentHealth <= 0);

            if (characterDies)
            {
                character = GetComponent<Character>();
                StartCoroutine(character.KillCharacter());
            }
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
            var pointsToAdd = maxManaReg * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana + pointsToAdd, 0, maxMana);
        }

        public void AddHealthPoints()
        {
            var pointsToAdd = maxHealthReg * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth + pointsToAdd, 0, maxHealth);
        }

        public void Heal(float points)
        {
            currentHealth = Mathf.Clamp(currentHealth + points, 0f, maxHealth);
        }
    }
}
