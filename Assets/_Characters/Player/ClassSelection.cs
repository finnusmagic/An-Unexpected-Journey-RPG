using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

public class ClassSelection : MonoBehaviour {

    [SerializeField] GameObject selectWarrior;
    [SerializeField] GameObject selectArcher;
    [SerializeField] GameObject selectMage;

    private void Start()
    {

        selectWarrior.GetComponent<Button>().onClick.AddListener(WarriorSelected);
        selectArcher.GetComponent<Button>().onClick.AddListener(ArcherSelected);
        selectMage.GetComponent<Button>().onClick.AddListener(MageSelected);
    }

    void WarriorSelected()
    {
        GameInfo.PlayerModel = 0;
    }
    void ArcherSelected()
    {
        GameInfo.PlayerModel = 1;
    }
    void MageSelected()
    {
        GameInfo.PlayerModel = 2;
    }
}
