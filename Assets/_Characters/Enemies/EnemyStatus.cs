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
        public Sprite enemyImage;
        public string enemyName;
        public int enemyLevel;
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

        GameObject enemyHealth;

        public float healthAsPercentage;

        public bool isAlive = true;

        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();

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

        IEnumerator KillPlayer()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);
            var playerComponent = GetComponent<Character>();
            if (playerComponent && playerComponent.isActiveAndEnabled) // relying on lazy evaluation
            {
                audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                audioSource.Play(); // overrind any existing sounds
                yield return new WaitForSecondsRealtime(audioSource.clip.length);
                SceneManager.LoadScene(0);
            }
            else // assume is enemy fr now, reconsider on other NPCs
            {
                DestroyObject(gameObject, deathVanishSeconds);
            }
        }

        IEnumerator KillEnemy()
        {
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