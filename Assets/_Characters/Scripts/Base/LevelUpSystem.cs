using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

public class LevelUpSystem : MonoBehaviour {

    GameObject levelBar;

    Text levelText;
    Text currentXPText;
    Image levelProgressBar;

    float fillAmount;
    float reverseFillAmount;

    int currentLevel;
    int baseXP = 20;
    int currentXP;

    int xpForNextLevel;
    int xpDifferenceToNextLevel;
    int totalXPDifference;

    float statPoints;
    float specialStatPoints;

    float levelDamage;
    float levelArmor;
    float levelHealth;
    float levelMana;

    float levelHealthReg;
    float levelManaReg;

    float levelCritChance;
    float levelCritDamage;

	void Start ()
    {
        levelBar = GameObject.Find("Panel - Player Level");

        VisualiseLevel();
        AddXP(0);
    }

    public void AddXP(int amount)
    {
        CalculateLevel(amount);
        VisualiseLevel();
    }

    void CalculateLevel(int amount)
    {
        currentXP += amount;

        int temp_cur_level = (int)Mathf.Sqrt(currentXP / baseXP) + 1;

        if (currentLevel != temp_cur_level)
        {
            currentLevel = temp_cur_level;
        }

        xpForNextLevel = baseXP * currentLevel * currentLevel;
        xpDifferenceToNextLevel = xpForNextLevel - currentXP;
        totalXPDifference = xpForNextLevel - (baseXP * (currentLevel -1) * (currentLevel -1));

        fillAmount = (float)xpDifferenceToNextLevel / (float)totalXPDifference;
        reverseFillAmount = 1 - fillAmount;

        statPoints = 5 * (currentLevel -1);
        specialStatPoints = 0.1f * (currentLevel - 1);

        PlayerLevelUp();
    }

    void PlayerLevelUp()
    {
        levelDamage = statPoints;
        levelArmor = statPoints;
        levelHealth = statPoints;
        levelMana = statPoints;

        levelHealthReg = specialStatPoints;
        levelManaReg = specialStatPoints;

        levelCritChance = specialStatPoints;
        levelCritDamage = specialStatPoints;
    }

    void VisualiseLevel()
    {
        levelText = levelBar.transform.GetChild(2).GetComponent<Text>();
        currentXPText = levelBar.transform.GetChild(3).GetComponent<Text>();
        levelProgressBar = levelBar.transform.GetChild(1).GetComponent<Image>();

        levelText.text = "Level: " + currentLevel.ToString();
        currentXPText.text = currentXP.ToString() + " / " + xpForNextLevel.ToString();
        levelProgressBar.fillAmount = reverseFillAmount;
    }	

    public float GetLevelDamage()
    {
        return levelDamage;
    }

    public float GetLevelArmor()
    {
        return levelArmor;
    }

    public float GetLevelHealth()
    {
        return levelHealth;
    }

    public float GetLevelMana()
    {
        return levelMana;
    }

    public float GetLevelHealthReg()
    {
        return levelHealthReg;
    }

    public float GetLevelManaReg()
    {
        return levelManaReg;
    }

    public float GetLevelCritChance()
    {
        return levelCritChance;
    }

    public float GetLevelCritDamage()
    {
        return levelCritDamage;
    }
}
