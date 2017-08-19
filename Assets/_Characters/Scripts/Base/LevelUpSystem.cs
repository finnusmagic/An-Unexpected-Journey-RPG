using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

public class LevelUpSystem : MonoBehaviour {

    [SerializeField] GameObject levelBar;
    Text levelText;
    Text currentXPText;
    Image levelProgressBar;

    private float fillAmount;
    private float reverseFillAmount;
    [Space(10)]
    public int xpToGive = 5;
    public int currentLevel;
    public int baseXP = 20;
    public int currentXP;

    public int xpForNextLevel;
    public int xpDifferenceToNextLevel;
    public int totalXPDifference;

    public float statPoints;
    public float skillPoints;

    public float levelStrength;
    public float levelDefense;
    public float levelHealth;
    public float levelMana;

	void Start ()
    {
        InvokeRepeating("AddXP", .5f, .5f);

        VisualiseLevel();
    }

    public void AddXP()
    {
        CalculateLevel(xpToGive);
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
        skillPoints = 5 * (currentLevel -1);

        PlayerLevelUp();
    }

    void PlayerLevelUp()
    {
        levelStrength = statPoints;
        levelDefense = statPoints;
        levelHealth = statPoints;
        levelMana = statPoints;
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
}
