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
        [SerializeField] AudioClip[] deathSounds = null;
        [SerializeField] float deathVanishSeconds = 2.0f;

        const string DEATH_TRIGGER = "Death";

        Animator animator;
        AudioSource audioSource;
        Character characterMovement;

        private static FloatingText popupText;

        GameObject enemyHealth;
        LevelUpSystem levelSystem;

        public float healthAsPercentage;

        public bool isAlive = true;

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

        int GetEnemyXP()
        {
            return xpToGive;
        }

        void Start()
        {
            levelSystem = FindObjectOfType<LevelUpSystem>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();

            currentHealthPoints = maxHealthPoints;

            InitializeFloatingText();
        }

        void Update()
        {
            UpdateHealthBar();
        }

        public static void InitializeFloatingText()
        {
            if (!popupText)
                popupText = Resources.Load<FloatingText>("Prefabs/Damage Number");
        }

        public void CreateFloatingText(string text, Transform location)
        {
            FloatingText instance = Instantiate(popupText);
            instance.transform.SetParent(transform, false);
            instance.SetText(text);
        }

        void UpdateHealthBar()
        {
            healthAsPercentage = currentHealthPoints / maxHealthPoints;
        }

        public void TakeDamage(float damage)
        {
            CreateFloatingText(damage.ToString(), transform);
            GetComponent<EnemyAI>().underAttack = true;

            bool characterDies = (currentHealthPoints - damage <= 0);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);

            if (characterDies)
            {
                StartCoroutine(KillEnemy());
            }

            UpdateHealthBar();
        }

        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }

        IEnumerator KillEnemy()
        {
            levelSystem.AddXP(GetEnemyXP());
            isAlive = false;
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);
            GetComponent<NavMeshAgent>().isStopped = true;

            var enemyComponent = GetComponent<Character>();
            if (enemyComponent && enemyComponent.isActiveAndEnabled)
            {
                audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                audioSource.Play();
                yield return new WaitForSecondsRealtime(audioSource.clip.length);
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }
}