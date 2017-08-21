using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class LockTarget : MonoBehaviour {

        public GameObject targetPanel;
        public EnemyAI target = null;

        private PlayerMovement playerMovement;
        private SpecialAbilities abilities;
        private WeaponSystem weaponSystem;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        private void Update()
        {
            if (target != null)
            {
                GetTarget();
                abilities.enemyTarget = target.gameObject;
                weaponSystem.target = target.gameObject;
            }
            if (target != null && target.GetComponent<EnemyStatus>().isAlive == false)
            {
                targetPanel.SetActive(false);
                abilities.enemyTarget = null;
                weaponSystem.target = null;
                target = null;
            }
        }

        void GetTarget()
        {
            var enemy = target.GetComponent<EnemyStatus>();
            targetPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = enemy.healthAsPercentage;
            targetPanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = (int)enemy.currentHealthPoints + " / " + enemy.maxHealthPoints;

            targetPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = enemy.GetEnemyImage();
            targetPanel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = enemy.GetEnemyName();
            targetPanel.transform.GetChild(1).GetChild(3).GetComponent<Text>().text = enemy.GetEnemyLevel().ToString();
        }
    }
}
