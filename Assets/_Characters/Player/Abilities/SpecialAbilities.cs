using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RPG.Database;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] List<AbilityConfig> abilities;

        private PlayerStatusManager player;
        public GameObject abilityPanel;

        InputManager inputManager;
        Character character;
        WeaponSystem weaponSystem;

        void Start()
        {
            if (inputManager == null)
                inputManager = (InputManager)Resources.Load("InputManager");

            player = GetComponent<PlayerStatusManager>();
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();

            AttachInitialAbilities();
            player.UpdatePlayerMana();
        }

        void FixedUpdate()
        {
            if (Input.GetKeyDown(inputManager.Ability_01_KeyCode))
            {
                if (abilities[0].currentCooldown >= abilities[0].coolDown && abilities[0].GetManaCost() <= player.currentMana)
                {
                    player.ConsumeMana(abilities[0].GetManaCost());
                    abilities[0].Use(gameObject);
                    abilities[0].currentCooldown = 0;
                }
            }
            else if (Input.GetKeyDown(inputManager.Ability_02_KeyCode))
            {
                if (abilities[1].currentCooldown >= abilities[1].coolDown && abilities[1].GetManaCost() <= player.currentMana)
                {
                    player.ConsumeMana(abilities[1].GetManaCost());
                    abilities[1].Use(gameObject);
                    abilities[1].currentCooldown = 0;
                }
            }
            else if (Input.GetKeyDown(inputManager.Ability_03_KeyCode) && GetComponent<LockTarget>().target != null)
            {
                EnemyAI enemyTarget = GetComponent<LockTarget>().target;

                if (!weaponSystem.IsTargetInRange(enemyTarget.gameObject) && enemyTarget != null)
                {
                    character.SetDestination(enemyTarget.transform.position);
                }

                else if (weaponSystem.IsTargetInRange(enemyTarget.gameObject) && enemyTarget != null)
                {
                    if (abilities[2].currentCooldown >= abilities[2].coolDown && abilities[2].GetManaCost() <= player.currentMana)
                    {
                        transform.LookAt(enemyTarget.GetComponent<EnemyAI>().transform);

                        player.ConsumeMana(abilities[2].GetManaCost());
                        abilities[2].Use(enemyTarget.gameObject);
                        abilities[2].currentCooldown = 0;
                    }
                }
            }
        }

        private void Update()
        {
            foreach (AbilityConfig a in abilities)
            {
                if (a.currentCooldown < a.coolDown)
                {
                    abilities[0].abilityIcon = abilityPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                    abilities[1].abilityIcon = abilityPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>();
                    abilities[2].abilityIcon = abilityPanel.transform.GetChild(2).GetChild(0).GetComponent<Image>();

                    a.currentCooldown += Time.deltaTime;
                    a.abilityIcon.fillAmount = a.currentCooldown / a.coolDown;
                }
            }

            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].GetManaCost() >= player.currentMana)
                {
                    abilityPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    abilityPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Count; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Count;
        }
    }
}