using UnityEngine;
using RPG.Characters;
using UnityEngine.UI;
using UnityEngine.AI;

namespace RPG.Database
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        [SerializeField] GameObject inventoryPanel;
        [SerializeField] GameObject characterSystemPanel;
        [SerializeField] GameObject craftSystemPanel;

        [Space(10)]
        [SerializeField] GameObject HandL;
        [SerializeField] GameObject HandR;
        WeaponConfig currentWeapon;

        private Inventory craftSystemInventory;
        private CraftSystem cS;
        private Inventory mainInventory;
        private Inventory characterSystemInventory;
        private Tooltip toolTip;

        private InputManager inputManagerDatabase;
        private PlayerStatsManager playerStatus;
        int normalSize = 3;

        float itemDamage;
        float itemArmor;
        float itemHealth;
        float itemMana;

        float itemHealthReg;
        float itemManaReg;

        float itemCritChance;
        float itemCritDamage;

        void Start()
        {
            playerStatus = FindObjectOfType<PlayerStatsManager>();

            if (inputManagerDatabase == null)
                inputManagerDatabase = (InputManager)Resources.Load("InputManager");

            if (craftSystemPanel != null)
                cS = craftSystemPanel.GetComponent<CraftSystem>();

            if (GameObject.FindGameObjectWithTag("Tooltip") != null)
                toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
            if (inventoryPanel != null)
                mainInventory = inventoryPanel.GetComponent<Inventory>();
            if (characterSystemPanel != null)
                characterSystemInventory = characterSystemPanel.GetComponent<Inventory>();
            if (craftSystemPanel != null)
                craftSystemInventory = craftSystemPanel.GetComponent<Inventory>();
        }

        void Update()
        {

            if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
            {
                if (!inventoryPanel.activeSelf)
                {
                    characterSystemInventory.openInventory();
                    mainInventory.openInventory();
                    this.GetComponent<PlayerMovement>().canMove = false;
                }
                else
                {
                    if (toolTip != null)
                        toolTip.deactivateTooltip();
                    characterSystemInventory.closeInventory();
                    mainInventory.closeInventory();
                    this.GetComponent<PlayerMovement>().canMove = true;
                }
            }

            if (inventoryPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                characterSystemInventory.closeInventory();
                mainInventory.closeInventory();
                this.GetComponent<PlayerMovement>().canMove = true;
            }

            if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
            {
                if (!craftSystemPanel.activeSelf)
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
                if (HandL.transform.childCount != 0)
                {
                    Destroy(HandL.transform.GetChild(0).gameObject);
                }
                if (HandR.transform.childCount != 0)
                {
                    Destroy(HandR.transform.GetChild(0).gameObject);
                }

                currentWeapon = GetComponent<WeaponSystem>().currentWeaponConfig = Resources.Load<WeaponConfig>("Weapons/" + item.itemName);

                if (currentWeapon.rightHanded)
                {
                    GameObject weaponObject = Instantiate(item.itemModel, HandR.transform, true);
                    weaponObject.transform.localPosition = currentWeapon.gripTransform.localPosition;
                    weaponObject.transform.localRotation = currentWeapon.gripTransform.localRotation;

                }
                if (currentWeapon.leftHanded)
                {
                    GameObject weaponObject = Instantiate(item.itemModel, HandL.transform, true);
                    weaponObject.transform.localPosition = currentWeapon.gripTransform.localPosition;
                    weaponObject.transform.localRotation = currentWeapon.gripTransform.localRotation;
                }
            }
        }

        void UnEquipWeapon(Item item)
        {
            if (item.itemType == ItemType.Weapon)
            {
                // TODO Fix
            }
        }

        void OnBackpack(Item item)
        {
            if (item.itemType == ItemType.Backpack)
            {
                for (int i = 0; i < item.itemAttributes.Count; i++)
                {
                    if (mainInventory == null)
                        mainInventory = inventoryPanel.GetComponent<Inventory>();
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
                mainInventory = inventoryPanel.GetComponent<Inventory>();
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
                    if ((playerStatus.CurrentHealth + item.itemAttributes[i].attributeValue) > playerStatus.MaxHealth)
                        playerStatus.CurrentHealth = playerStatus.MaxHealth;
                    else
                        playerStatus.CurrentHealth += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Mana")
                {
                    if ((playerStatus.CurrentMana + item.itemAttributes[i].attributeValue) > playerStatus.MaxMana)
                        playerStatus.CurrentMana = playerStatus.MaxMana;
                    else
                        playerStatus.CurrentMana += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Armor")
                {
                    if ((playerStatus.MaxArmor + item.itemAttributes[i].attributeValue) > playerStatus.MaxArmor)
                        playerStatus.MaxArmor = playerStatus.MaxArmor;
                    else
                        playerStatus.MaxArmor += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Damage")
                {
                    if ((playerStatus.MaxDamage + item.itemAttributes[i].attributeValue) > playerStatus.MaxDamage)
                        playerStatus.MaxDamage = playerStatus.MaxDamage;
                    else
                        playerStatus.MaxDamage += item.itemAttributes[i].attributeValue;
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
                {
                    itemHealth += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Mana")
                {
                    itemMana += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Armor")
                {
                    itemArmor += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Damage")
                {
                    itemDamage += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Health Reg")
                {
                    itemHealthReg += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Mana Reg")
                {
                    itemManaReg += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Crit Chance")
                {
                    itemCritChance += item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Crit Damage")
                {
                    itemCritDamage += item.itemAttributes[i].attributeValue;
                }
            }
        }

        public void OnUnEquipItem(Item item)
        {
            for (int i = 0; i < item.itemAttributes.Count; i++)
            {
                if (item.itemAttributes[i].attributeName == "Health")
                {
                    itemHealth -= item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Mana")
                {
                    itemMana -= item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Armor")
                {
                    itemArmor -= item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Damage")
                {
                    itemDamage -= item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Health Reg")
                {
                    itemHealthReg -= item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Mana Reg")
                {
                    itemManaReg -= item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Crit Chance")
                {
                    itemCritChance -= item.itemAttributes[i].attributeValue;
                }
                if (item.itemAttributes[i].attributeName == "Crit Damage")
                {
                    itemCritDamage -= item.itemAttributes[i].attributeValue;
                }
            }
        }

        public GameObject GetInventoryPanel()
        {
            return inventoryPanel;
        }

        public GameObject GetCharacterPanel()
        {
            return characterSystemPanel;
        }

        public GameObject GetCraftSystemPanel()
        {
            return craftSystemPanel;
        }

        public float GetItemDamage()
        {
            return itemDamage;
        }

        public float GetItemArmor()
        {
            return itemArmor;
        }

        public float GetItemHealth()
        {
            return itemHealth;
        }

        public float GetItemMana()
        {
            return itemMana;
        }

        public float GetItemHealthReg()
        {
            return itemHealthReg;
        }

        public float GetItemManaReg()
        {
            return itemManaReg;
        }

        public float GetItemCritChance()
        {
            return itemCritChance;
        }

        public float GetItemCritDamage()
        {
            return itemCritDamage;
        }
    }
}

