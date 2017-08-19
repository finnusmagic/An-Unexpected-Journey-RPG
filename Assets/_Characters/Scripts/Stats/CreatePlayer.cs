using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreatePlayer : MonoBehaviour {

    private BasePlayerClass newPlayer;
    private string playerName = "Test Player";

    public Text damageText = null;
    public Text armorText = null;
    public Text healthText = null;
    public Text manaText = null;

    public Text healthRegText = null;
    public Text manaRegText = null;

    public Text critChanceText = null;
    public Text critDamageText = null;

    public Text classNameText = null;
    public Text classDescriptionText = null;
    public Image classIconSprite;

    string className;
    string classDescription;
    Sprite classIcon;


    private int pointsToSpend = 20;
    public Text pointsText;

    private void Start()
    {
        newPlayer = new BasePlayerClass();
        classIcon = Resources.Load<Sprite>("Sprites/Classes/noSelection");

        UpdateUI();
    }

    public void CreateNewPlayer()
    {
        newPlayer.PlayerLevel = 1;
        newPlayer.PlayerName = playerName;

        GameInfo.PlayerLevel = newPlayer.PlayerLevel;
        GameInfo.PlayerName = newPlayer.PlayerName;
        GameInfo.PlayerClass = newPlayer.PlayerClass;

        GameInfo.Damage = newPlayer.Damage;
        GameInfo.Armor = newPlayer.Armor;
        GameInfo.Health = newPlayer.Health;
        GameInfo.Mana = newPlayer.Mana;

        GameInfo.HealthRegen = newPlayer.HealthReg;
        GameInfo.ManaRegen = newPlayer.ManaReg;

        GameInfo.CritChance = newPlayer.CritChance;
        GameInfo.CritDamage = newPlayer.CritDamage;

        SaveInfo.SaveAllInfo();

        SceneManager.LoadScene("02_Start_Game_Scene");
    }

    public void SetArcherClass()
    {
        pointsToSpend = 20;
        newPlayer.PlayerClass = new BaseArcherClass();
        newPlayer.Damage = newPlayer.PlayerClass.Damage;
        newPlayer.Armor = newPlayer.PlayerClass.Armor;
        newPlayer.Health = newPlayer.PlayerClass.Health;
        newPlayer.Mana = newPlayer.PlayerClass.Mana;

        newPlayer.HealthReg = newPlayer.PlayerClass.HealthReg;
        newPlayer.ManaReg = newPlayer.PlayerClass.ManaReg;

        newPlayer.CritChance = newPlayer.PlayerClass.CritChance;
        newPlayer.CritDamage = newPlayer.PlayerClass.CritDamage;

        className = "The Archer";
        classDescription = "The Archer is a ranged Class that focuses on mastering the bow. His shooting skills are the best out there and his agility is the greatest.";
        classIcon = Resources.Load<Sprite>("Sprites/Classes/archer");

        UpdateUI();
    }
    public void SetWarriorClass()
    {
        newPlayer.PlayerClass = new BaseWarriorClass();
        newPlayer.Damage = newPlayer.PlayerClass.Damage;
        newPlayer.Armor = newPlayer.PlayerClass.Armor;
        newPlayer.Health = newPlayer.PlayerClass.Health;
        newPlayer.Mana = newPlayer.PlayerClass.Mana;

        newPlayer.HealthReg = newPlayer.PlayerClass.HealthReg;
        newPlayer.ManaReg = newPlayer.PlayerClass.ManaReg;

        newPlayer.CritChance = newPlayer.PlayerClass.CritChance;
        newPlayer.CritDamage = newPlayer.PlayerClass.CritDamage;

        className = "The Warrior";
        classDescription = "The Warrior is a meele Class that focuses on mastering the Sword. His sword skills are the strongest out there and his defence is the greatest";
        classIcon = Resources.Load<Sprite>("Sprites/Classes/warrior");

        UpdateUI();
    }
    public void SetMageClass()
    {
        newPlayer.PlayerClass = new BaseMageClass();
        newPlayer.Damage = newPlayer.PlayerClass.Damage;
        newPlayer.Armor = newPlayer.PlayerClass.Armor;
        newPlayer.Health = newPlayer.PlayerClass.Health;
        newPlayer.Mana = newPlayer.PlayerClass.Mana;

        newPlayer.HealthReg = newPlayer.PlayerClass.HealthReg;
        newPlayer.ManaReg = newPlayer.PlayerClass.ManaReg;

        newPlayer.CritChance = newPlayer.PlayerClass.CritChance;
        newPlayer.CritDamage = newPlayer.PlayerClass.CritDamage;

        className = "The Mage";
        classDescription = "The Mage is a Class that focuses on casting strong abilities to destruct their enemies. His abilities are his greatest Power, but his defense is his greates weakness. ";
        classIcon = Resources.Load<Sprite>("Sprites/Classes/mage");

        UpdateUI();
    }

    void UpdateUI()
    {
        damageText.text = newPlayer.Damage.ToString();
        armorText.text = newPlayer.Armor.ToString();
        healthText.text = newPlayer.Health.ToString();
        manaText.text = newPlayer.Mana.ToString();

        healthRegText.text = newPlayer.HealthReg.ToString();
        manaRegText.text = newPlayer.ManaReg.ToString();

        critChanceText.text = newPlayer.CritChance.ToString();
        critDamageText.text = newPlayer.CritDamage.ToString();

        pointsText.text = pointsToSpend.ToString();

        classNameText.text = className;
        classDescriptionText.text = classDescription;
        classIconSprite.sprite = classIcon;
    }

    public void SetDamage(int amount)
    {
        if (newPlayer.PlayerClass != null)
        {
            if (amount > 0 && pointsToSpend > 0)
            {
                newPlayer.Damage += amount;
                pointsToSpend -= 1;
                UpdateUI();
            } 
            else if (amount < 0 && newPlayer.Damage > newPlayer.PlayerClass.Damage)
            {
                newPlayer.Damage += amount;
                pointsToSpend += 1;
                UpdateUI();
            }
        }
        else
        {
            Debug.Log("No Class Choosen!");
        }
    }
    public void SetArmor(int amount)
    {
        if (newPlayer.PlayerClass != null)
        {
            if (amount > 0 && pointsToSpend > 0)
            {
                newPlayer.Armor += amount;
                pointsToSpend -= 1;
                UpdateUI();
            }
            else if (amount < 0 && newPlayer.Armor > newPlayer.PlayerClass.Armor)
            {
                newPlayer.Armor += amount;
                pointsToSpend += 1;
                UpdateUI();
            }
        }
        else
        {
            Debug.Log("No Class Choosen!");
        }
    }
    public void SetHealth(int amount)
    {
        if (newPlayer.PlayerClass != null)
        {
            if (amount > 0 && pointsToSpend > 0)
            {
                newPlayer.Health += amount;
                pointsToSpend -= 1;
                UpdateUI();
            }
            else if (amount < 0 && newPlayer.Health > newPlayer.PlayerClass.Health)
            {
                newPlayer.Health += amount;
                pointsToSpend += 1;
                UpdateUI();
            }
        }
        else
        {
            Debug.Log("No Class Choosen!");
        }
    }
    public void SetMana(int amount)
    {
        if (newPlayer.PlayerClass != null)
        {
            if (amount > 0 && pointsToSpend > 0)
            {
                newPlayer.Mana += amount;
                pointsToSpend -= 1;
                UpdateUI();
            }
            else if (amount < 0 && newPlayer.Mana > newPlayer.PlayerClass.Mana)
            {
                newPlayer.Mana += amount;
                pointsToSpend += 1;
                UpdateUI();
            }
        }
        else
        {
            Debug.Log("No Class Choosen!");
        }
    }

    public void LoadClassStats()
    {
        LoadInfo.LoadAllInfo();
    }
}
