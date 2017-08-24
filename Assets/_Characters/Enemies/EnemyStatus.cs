using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class EnemyStatus : MonoBehaviour
    {
        [Header("Enemy Information")]
        [SerializeField]
        Class currentClass;
        [Space(10)]
        [SerializeField]
        Sprite enemyImage;
        [SerializeField] string enemyName;
        [SerializeField] int enemyLevel;
        [SerializeField] int xpToGive;
        [Space(10)]
        public float maxHealthPoints = 1000;
        public float currentHealthPoints;

        [Header("Enemy Setup")]
        Character character;

        GameObject enemyHealth;

        public enum Class { Archer, Swordfighter, AxeFighter }

        public float healthAsPercentage;

        public Sprite GetEnemyImage()
        {
            return enemyImage;
        }

        public string GetEnemyName()
        {
            return enemyName;
        }

        public int GetEnemyLevel()
        {
            return enemyLevel;
        }

        public int GetEnemyXP()
        {
            return xpToGive;
        }

        void Start()
        {
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            UpdateHealthBar();
        }

        void UpdateHealthBar()
        {
            healthAsPercentage = currentHealthPoints / maxHealthPoints;
        }

        public void TakeDamage(float damage)
        {
            CheckForDamageSounds();

            character = GetComponent<Character>();
            character.CreateFloatingText(damage.ToString(), transform);

            currentHealthPoints = currentHealthPoints - damage;

            bool characterDies = (currentHealthPoints <= 0);

            if (characterDies)
            {
                CheckForDeathSounds();
                character = GetComponent<Character>();
                StartCoroutine(character.KillCharacter());
            }

            UpdateHealthBar();
            StartCoroutine(GettingAttacked());
        }

        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }

        IEnumerator GettingAttacked()
        {
            GetComponent<EnemyAI>().gettingAttacked = true;
            yield return new WaitForSeconds(4f);
            GetComponent<EnemyAI>().gettingAttacked = false;
        }

        void CheckForDamageSounds()
        {
            AudioManager audioManager = AudioManager.instance;

            if (currentClass == Class.Archer)
            {
                audioManager.PlaySound("Archer Damage");
            }
            if (currentClass == Class.Swordfighter)
            {
                audioManager.PlaySound("Swordfighter Damage");
            }
            if (currentClass == Class.AxeFighter)
            {
                audioManager.PlaySound("Axefighter Damage");
            }
        }

        void CheckForDeathSounds()
        {
            AudioManager audioManager = AudioManager.instance;

            if (currentClass == Class.Archer)
            {
                audioManager.PlaySound("Archer Death");
            }
            if (currentClass == Class.Swordfighter)
            {
                audioManager.PlaySound("Swordfighter Death");
            }
            if (currentClass == Class.AxeFighter)
            {
                audioManager.PlaySound("Axefighter Death");
            }
        }

        public void CheckForTriggerSounds()
        {
            AudioManager audioManager = AudioManager.instance;

            if (currentClass == Class.Archer)
            {
                audioManager.PlaySound("Archer Trigger");
            }
            if (currentClass == Class.Swordfighter)
            {
                audioManager.PlaySound("Swordfighter Trigger");
            }
            if (currentClass == Class.AxeFighter)
            {
                audioManager.PlaySound("Axefighter Trigger");
            }
        }
    }
}