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
        [SerializeField] Sprite enemyImage;
        [SerializeField] string enemyName;
        [SerializeField] int enemyLevel;
        [SerializeField] int xpToGive;
        [Space(10)]
        public float maxHealthPoints = 1000;
        public float currentHealthPoints;

        [Header("Enemy Setup")]
        [SerializeField] AudioClip[] damageSounds = null;
        AudioSource audioSource;
        Character character;

        GameObject enemyHealth;

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
            audioSource = GetComponent<AudioSource>();

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
            character = GetComponent<Character>();
            character.CreateFloatingText(damage.ToString(), transform);
            GetComponent<EnemyAI>().underAttack = true;

            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);

            bool characterDies = (currentHealthPoints - damage <= 0);

            if (characterDies)
            {
                character = GetComponent<Character>();
                StartCoroutine(character.KillCharacter());
            }

            UpdateHealthBar();
        }

        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }
    }
}