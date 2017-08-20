using UnityEngine;
using RPG.Characters;
using UnityEngine.UI;
using UnityEngine.AI;

namespace RPG.Database
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        public GameObject inventory;
        public GameObject characterSystem;
        public GameObject craftSystem;
        [Space(10)]
        public DominantHand weaponHandR;
        public DominantHand weaponHandL;

        private Inventory craftSystemInventory;
        private CraftSystem cS;
        private Inventory mainInventory;
        private Inventory characterSystemInventory;
        private Tooltip toolTip;

        private InputManager inputManagerDatabase;
        private PlayerStatusManager playerStatus;
        int normalSize = 3;

        public float itemDamage;
        public float itemArmor;
        public float itemHealth;
        public float itemMana;

        public float itemHealthReg;
        public float itemManaReg;

        public float itemCritChance;
        public float itemCritDamage;

        void Start()
        {
            playerStatus = FindObjectOfType<PlayerStatusManager>();

            if (inputManagerDatabase == null)
                inputManagerDatabase = (InputManager)Resources.Load("InputManager");

            if (craftSystem != null)
                cS = craftSystem.GetComponent<CraftSystem>();

            if (GameObject.FindGameObjectWithTag("Tooltip") != null)
                toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
            if (inventory != null)
                mainInventory = inventory.GetComponent<Inventory>();
            if (characterSystem != null)
                characterSystemInventory = characterSystem.GetComponent<Inventory>();
            if (craftSystem != null)
                craftSystemInventory = craftSystem.GetComponent<Inventory>();
        }

        void Update()
        {
            if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode))
            {
                if (!characterSystem.activeSelf)
                {
                    characterSystemInventory.openInventory();
                }
                else
                {
                    if (toolTip != null)
                        toolTip.deactivateTooltip();
                    characterSystemInventory.closeInventory();
                }
            }

            if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
            {
                if (!inventory.activeSelf)
                {
                    mainInventory.openInventory();
                    this.GetComponent<PlayerMovement>().canMove = false;
                }
                else
                {
                    if (toolTip != null)
                        toolTip.deactivateTooltip();
                    mainInventory.closeInventory();
                    this.GetComponent<PlayerMovement>().canMove = true;
                }
            }

            if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
            {
                if (!craftSystem.activeSelf)
                    craftSystemInventory.openInventory();
                else
                {
                    if (cS != null)
                        cS.backToInventory();
                    if (toolTip != null)
                        toolTip.deactivateTooltip();
                    craftSystemInventory.closeInventory();
                }
            }

        }

        public void OnEnable()
        {
            Inventory.ItemEquip += OnBackpack;
            Inventory.UnEquipItem += UnEquipBackpack;

            Inventory.ItemEquip += OnGearItem;
            Inventory.ItemConsumed += OnConsumeItem;
            Inventory.UnEquipItem += OnUnEquipItem;

            Inventory.ItemEquip += EquipWeapon;
            Inventory.UnEquipItem += UnEquipWeapon;
        }

        public void OnDisable()
        {
            Inventory.ItemEquip -= OnBackpack;
            Inventory.UnEquipItem -= UnEquipBackpack;

            Inventory.ItemEquip -= OnGearItem;
            Inventory.ItemConsumed -= OnConsumeItem;
            Inventory.UnEquipItem -= OnUnEquipItem;

            Inventory.UnEquipItem -= UnEquipWeapon;
            Inventory.ItemEquip -= EquipWeapon;
        }

        void EquipWeapon(Item item)
        {
            if (item.itemType == ItemType.Weapon)
            {
                var currentWeapon = GetComponent<WeaponSystem>().currentWeaponConfig = Resources.Load<WeaponConfig>("Weapons/" + item.itemName);
                if (!currentWeapon.isRanged)
                {
                    GameObject weaponObject = Instantiate(item.itemModel, weaponHandR.transform, true);
                    weaponObject.transform.localPosition = currentWeapon.gripTransform.localPosition;
                    weaponObject.transform.localRotation = currentWeapon.gripTransform.localRotation;
                }
                if (currentWeapon.isRanged)
                {
                    GameObject weaponObject = Instantiate(item.itemModel, weaponHandL.transform, true);
                    weaponObject.transform.localPosition = currentWeapon.gripTransform.localPosition;
                    weaponObject.transform.localRotation = currentWeapon.gripTransform.localRotation;
                }
            }
        }

        void UnEquipWeapon(Item item)
        {
            if (item.itemType == ItemType.Weapon)
            {
                var currentWeapon = GetComponent<WeaponSystem>().currentWeaponConfig = Resources.Load<WeaponConfig>("Weapons/" + item.itemName);
                if (!currentWeapon.isRanged)
                {
                    Destroy(weaponHandR.transform.GetChild(3).gameObject);
                }
                if (currentWeapon.isRanged)
                {
                    Destroy(weaponHandL.transform.GetChild(3).gameObject);
                }
            }
        }

        void OnBackpack(Item item)
        {
            if (item.itemType == ItemType.Backpack)
            {
                for (int i = 0; i < item.itemAttributes.Count; i++)
                {
                    if (mainInventory == null)
                        mainInventory = inventory.GetComponent<Inventory>();
                    mainInventory.sortItems();
                    if (item.itemAttributes[i].attributeName == "Slots")
                        ChangeInventorySize((int)item.itemAttributes[i].attributeValue);
                }
            }
        }

        void UnEquipBackpack(Item item)
        {
            if (item.itemType == ItemType.Backpack)
                ChangeInventorySize(normalSize);
        }

        void ChangeInventorySize(int size)
        {
            DropTheRestItems(size);

            if (mainInventory == null)
                mainInventory = inventory.GetComponent<Inventory>();
            if (size == 3)
            {
                mainInventory.width = 3;
                mainInventory.height = 1;
                mainInventory.updateSlotAmount();
                mainInventory.adjustInventorySize();
            }
            if (size == 6)
            {
                mainInventory.width = 3;
                mainInventory.height = 2;
                mainInventory.updateSlotAmount();
                mainInventory.adjustInventorySize();
            }
            else if (size == 12)
            {
                mainInventory.width = 4;
                mainInventory.height = 3;
                mainInventory.updateSlotAmount();
                mainInventory.adjustInventorySize();
            }
            else if (size == 16)
            {
                mainInventory.width = 4;
                mainInventory.height = 4;
                mainInventory.updateSlotAmount();
                mainInventory.adjustInventorySize();
            }
            else if (size == 24)
            {
                mainInventory.width = 6;
                mainInventory.height = 4;
                mainInventory.updateSlotAmount();
                mainInventory.adjustInventorySize();
            }
        }

        void DropTheRestItems(int size)
        {
            if (size < mainInventory.ItemsInInventory.Count)
            {
                for (int i = size; i < mainInventory.ItemsInInventory.Count; i++)
                {
                    GameObject dropItem = (GameObject)Instantiate(mainInventory.ItemsInInventory[i].itemModel);
                    dropItem.AddComponent<PickUpItem>();
                    dropItem.GetComponent<PickUpItem>().item = mainInventory.ItemsInInventory[i];
                    dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
                }
            }
        }

        public void OnConsumeItem(Item item)
        {
            for (int i = 0; i < item.itemAttributes.Count; i++)
            {
                if (item.itemAttributes[i].attributeName == "Health")
                {
                    if ((playerStatus.currentHealth + item.itemAttributes[i].attributeValue) > playerStatus.maxHealth)
                        playerStatus.currentHealth = playerStatus.maxHealth;
                    else
                        playerStatus.currentHealth += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Mana")
                {
                    if ((playerStatus.currentMana + item.itemAttributes[i].attributeValue) > playerStatus.maxMana)
                        playerStatus.currentMana = playerStatus.maxMana;
                    else
                        playerStatus.currentMana += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Armor")
                {
                    if ((playerStatus.currentArmor + item.itemAttributes[i].attributeValue) > playerStatus.maxArmor)
                        playerStatus.currentArmor = playerStatus.maxArmor;
                    else
                        playerStatus.currentArmor += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Damage")
                {
                    if ((playerStatus.currentDamage + item.itemAttributes[i].attributeValue) > playerStatus.maxDamage)
                        playerStatus.currentDamage = playerStatus.maxDamage;
                    else
                        playerStatus.currentDamage += item.itemAttributes[i].attributeValue;
                }
                playerStatus.UpdatePlayerHealth();
                playerStatus.UpdatePlayerMana();
            }
        }

        public void OnGearItem(Item item)
        {
            for (int i = 0; i < item.itemAttributes.Count; i++)
            {
                if (item.itemAttributes[i].attributeName == "Health")
                    itemHealth += item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Mana")
                    itemMana += item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Armor")
                    itemArmor += item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Damage")
                    itemDamage += item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Health Reg")
                    itemHealthReg += item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Mana Reg")
                    itemManaReg += item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Crit Chance")
                    itemCritChance += item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Crit Damage")
                    itemCritDamage += item.itemAttributes[i].attributeValue;
            }
        }

        public void OnUnEquipItem(Item item)
        {
            for (int i = 0; i < item.itemAttributes.Count; i++)
            {
                if (item.itemAttributes[i].attributeName == "Health")
                    itemHealth -= item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Mana")
                    itemMana -= item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Armor")
                    itemArmor -= item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Damage")
                    itemDamage -= item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Health Reg")
                    itemHealthReg -= item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Mana Reg")
                    itemManaReg -= item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Crit Chance")
                    itemCritChance -= item.itemAttributes[i].attributeValue;
                if (item.itemAttributes[i].attributeName == "Crit Damage")
                    itemCritDamage -= item.itemAttributes[i].attributeValue;
            }
        }

    }
}

