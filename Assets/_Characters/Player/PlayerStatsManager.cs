using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Database;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG.Characters
{
    public class PlayerStatsManager : MonoBehaviour
    {
        GameObject playerMesh;

        [SerializeField] Mesh warriorMesh;
        [SerializeField] Mesh archerMesh;
        [SerializeField] Mesh mageMesh;

        [SerializeField] GameObject Inventory;

        float currentHealth;
        float currentMana;

        float maxHealth;
        float maxMana;

        float maxDamage;
        float maxArmor;

        float maxHealthReg;
        float maxManaReg;

        float maxCritChance;
        float maxCritDamage;

        Text hpText;
        Text manaText;

        Image hpImage;
        Image manaImage;

        Text Damage;
        Text Armor;
        Text Health;
        Text Mana;
        Text HealthReg;
        Text ManaReg;
        Text CritChance;
        Text CritDamage;

        private LevelUpSystem levelStats;
        private PlayerInventoryManager itemStats;
        private Character character;

        [SerializeField] GameObject playerStatusPanel;
        [SerializeField] GameObject playerStatsPanel;

        public float healthAsPercentage;

        private void Start()
        {
            InstantiatePlayerModel();

            levelStats = GetComponent<LevelUpSystem>();
            itemStats = GetComponent<PlayerInventoryManager>();

            if (playerStatusPanel != null)
            {
                SetupUIElements();
                UpdatePlayerStats();
            }

            currentHealth = maxHealth;
            currentMana = maxMana;
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
            float damageBeforeCritical = maxDamage;
            float TotalCritDamage = damageBeforeCritical * (maxCritDamage) / 100;
            float TotalCritChance = maxCritChance;
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

        public void UpdatePlayerStats()
        {
            healthAsPercentage = currentHealth / maxHealth;

            MaxDamage = GameInfo.Damage + itemStats.GetItemDamage() + levelStats.GetLevelDamage();
            MaxArmor = GameInfo.Armor + itemStats.GetItemArmor() + levelStats.GetLevelArmor();
            maxHealth = GameInfo.Health + itemStats.GetItemHealth() + levelStats.GetLevelHealth();
            maxMana = GameInfo.Mana + itemStats.GetItemMana() + levelStats.GetLevelMana();

            maxHealthReg = GameInfo.HealthRegen + itemStats.GetItemHealthReg() + levelStats.GetLevelHealthReg();
            maxManaReg = GameInfo.ManaRegen + itemStats.GetItemManaReg() + levelStats.GetLevelManaReg();

            maxCritChance = GameInfo.CritChance + itemStats.GetItemCritChance() + levelStats.GetLevelCritChance();
            maxCritDamage = GameInfo.CritDamage + itemStats.GetItemCritDamage() + levelStats.GetLevelCritDamage();

            Damage.text = MaxDamage.ToString();
            Armor.text = MaxArmor.ToString();
            Health.text = maxHealth.ToString();
            Mana.text = maxMana.ToString();

            HealthReg.text = maxHealthReg.ToString() + " %";
            ManaReg.text = maxManaReg.ToString() + " %";

            CritChance.text = maxCritChance.ToString() + " %";
            CritDamage.text = maxCritDamage.ToString() + " %";

            UpdatePlayerMana();
            UpdatePlayerHealth();
        }

        void SetupUIElements()
        {
            hpText = playerStatusPanel.transform.GetChild(0).GetChild(3).GetComponent<Text>();
            hpImage = playerStatusPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>();
            manaText = playerStatusPanel.transform.GetChild(1).GetChild(3).GetComponent<Text>();
            manaImage = playerStatusPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>();

            Damage = playerStatsPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>();
            Armor = playerStatsPanel.transform.GetChild(2).GetChild(1).GetComponent<Text>();
            Health = playerStatsPanel.transform.GetChild(2).GetChild(2).GetComponent<Text>();
            Mana = playerStatsPanel.transform.GetChild(2).GetChild(3).GetComponent<Text>();

            HealthReg = playerStatsPanel.transform.GetChild(4).GetChild(2).GetComponent<Text>();
            ManaReg = playerStatsPanel.transform.GetChild(4).GetChild(3).GetComponent<Text>();

            CritChance = playerStatsPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>();
            CritDamage = playerStatsPanel.transform.GetChild(4).GetChild(1).GetComponent<Text>();
        }

        public void TakeDamage(float damage)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlaySound("Player Getting Damage");

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

        private void InstantiatePlayerModel()
        {
            Inventory.GetComponent<Inventory>();

            playerMesh = GameObject.Find("Player Mesh");

            AudioManager audioManager = AudioManager.instance;
            Scene scene = SceneManager.GetActiveScene();
            audioManager.StopMusic("01_CharacterCreation_Scene");
            audioManager.PlayMusic(scene.name);

            if (GameInfo.PlayerModel == 0)
            {
                playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = warriorMesh;

                Inventory.GetComponent<Inventory>().addItemToInventory(4, 4);
                Inventory.GetComponent<Inventory>().addItemToInventory(5, 4);
                Inventory.GetComponent<Inventory>().addItemToInventory(1);
                Inventory.GetComponent<Inventory>().addItemToInventory(2);
                Inventory.GetComponent<Inventory>().addItemToInventory(3);
                Inventory.GetComponent<Inventory>().addItemToInventory(7);
            }
            if (GameInfo.PlayerModel == 1)
            {
                playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = archerMesh;
                Inventory.GetComponent<Inventory>().addItemToInventory(2);
                Inventory.GetComponent<Inventory>().addItemToInventory(4, 2);
                Inventory.GetComponent<Inventory>().addItemToInventory(5);
            }
            if (GameInfo.PlayerModel == 2)
            {
                playerMesh.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = mageMesh;
                Inventory.GetComponent<Inventory>().addItemToInventory(3);
                Inventory.GetComponent<Inventory>().addItemToInventory(5, 2);
                Inventory.GetComponent<Inventory>().addItemToInventory(4);
            }
        }


        public float CurrentHealth
        {
            get { return currentHealth; } 
            set { currentHealth = value; }
        }

        public float MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public float CurrentMana
        {
            get { return currentMana; }
            set { currentMana = value; }
        }

        public float MaxMana
        {
            get { return maxMana; }
            set { maxMana = value; }
        }

        public float MaxDamage
        {
            get { return maxDamage; }
            set { maxDamage = value; }
        }

        public float MaxArmor
        {
            get { return maxArmor; }
            set { maxArmor = value; }
        }

        public float MaxHealthReg
        {
            get { return maxHealthReg; }
            set { maxHealthReg = value; }
        }

        public float MaxManaReg
        {
            get { return maxManaReg; }
            set { maxManaReg = value; }
        }

        public float MaxCritChance
        {
            get { return maxCritChance; }
            set { maxCritChance = value; }
        }

        public float MaxCritDamage
        {
            get { return maxCritDamage; }
            set { maxCritDamage = value; }
        }
    }
}
