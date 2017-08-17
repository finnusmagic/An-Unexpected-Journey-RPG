using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreatePlayer : MonoBehaviour {

    private BasePlayerClass newPlayer;
    private string playerName = "Test Player";

    public Text strengthText = null;
    public Text defenseText = null;
    public Text healthText = null;
    public Text manaText = null;

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

        GameInfo.Strength = newPlayer.Strength;
        GameInfo.Defense = newPlayer.Defense;
        GameInfo.Health = newPlayer.Health;
        GameInfo.Mana = newPlayer.Mana;

        SaveInfo.SaveAllInfo();

        SceneManager.LoadScene("02_Start_Game_Scene");
    }

    public void SetArcherClass()
    {
        pointsToSpend = 20;
        newPlayer.PlayerClass = new BaseArcherClass();
        newPlayer.Strength = newPlayer.PlayerClass.Strength;
        newPlayer.Defense = newPlayer.PlayerClass.Defense;
        newPlayer.Health = newPlayer.PlayerClass.Health;
        newPlayer.Mana = newPlayer.PlayerClass.Mana;

        className = "The Archer";
        classDescription = "The Archer is a ranged Class that focuses on mastering the bow. His shooting skills are the best out there and his agility is the greatest.";
        classIcon = Resources.Load<Sprite>("Sprites/Classes/archer");

        UpdateUI();
    }
    public void SetWarriorClass()
    {
        newPlayer.PlayerClass = new BaseWarriorClass();
        newPlayer.Strength = newPlayer.PlayerClass.Strength;
        newPlayer.Defense = newPlayer.PlayerClass.Defense;
        newPlayer.Health = newPlayer.PlayerClass.Health;
        newPlayer.Mana = newPlayer.PlayerClass.Mana;

        className = "The Warrior";
        classDescription = "The Warrior is a meele Class that focuses on mastering the Sword. His sword skills are the strongest out there and his defence is the greatest";
        classIcon = Resources.Load<Sprite>("Sprites/Classes/warrior");

        UpdateUI();
    }
    public void SetMageClass()
    {
        newPlayer.PlayerClass = new BaseMageClass();
        newPlayer.Strength = newPlayer.PlayerClass.Strength;
        newPlayer.Defense = newPlayer.PlayerClass.Defense;
        newPlayer.Health = newPlayer.PlayerClass.Health;
        newPlayer.Mana = newPlayer.PlayerClass.Mana;

        className = "The Mage";
        classDescription = "The Mage is a Class that focuses on casting strong abilities to destruct their enemies. His abilities are his greatest Power, but his defense is his greates weakness. ";
        classIcon = Resources.Load<Sprite>("Sprites/Classes/mage");

        UpdateUI();
    }

    void UpdateUI()
    {
        strengthText.text = newPlayer.Strength.ToString();
        defenseText.text = newPlayer.Defense.ToString();
        healthText.text = newPlayer.Health.ToString();
        manaText.text = newPlayer.Mana.ToString();

        pointsText.text = pointsToSpend.ToString();

        classNameText.text = className;
        classDescriptionText.text = classDescription;
        classIconSprite.sprite = classIcon;
    }

    public void SetStrength(int amount)
    {
        if (newPlayer.PlayerClass != null)
        {
            if (amount > 0 && pointsToSpend > 0)
            {
                newPlayer.Strength += amount;
                pointsToSpend -= 1;
                UpdateUI();
            } 
            else if (amount < 0 && newPlayer.Strength > newPlayer.PlayerClass.Strength)
            {
                newPlayer.Strength += amount;
                pointsToSpend += 1;
                UpdateUI();
            }
        }
        else
        {
            Debug.Log("No Class Choosen!");
        }
    }
    public void SetDefense(int amount)
    {
        if (newPlayer.PlayerClass != null)
        {
            if (amount > 0 && pointsToSpend > 0)
            {
                newPlayer.Defense += amount;
                pointsToSpend -= 1;
                UpdateUI();
            }
            else if (amount < 0 && newPlayer.Defense > newPlayer.PlayerClass.Defense)
            {
                newPlayer.Defense += amount;
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
