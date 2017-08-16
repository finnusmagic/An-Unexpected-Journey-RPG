using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] bool isEnemy;
        [SerializeField] GameObject enemyCanvas = null;
        Camera targetCamera;
        [Space(10)]
        [SerializeField] float maxHealthPoints = 100f;
       // [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float deathVanishSeconds = 2.0f;

        const string DEATH_TRIGGER = "Death";

        float currentHealthPoints;
        Animator animator;
        AudioSource audioSource;
        Character characterMovement;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            targetCamera = Camera.main;
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();

            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            UpdateHealthBar();

            if (isEnemy)
            {
                enemyCanvas.transform.rotation = targetCamera.transform.rotation;
            }
        }

        void UpdateHealthBar()
        {
           // if (healthBar) // Enemies may not have health bars to update
           // {
               // healthBar.fillAmount = healthAsPercentage;
           // }
        }

        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage <= 0);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);

            if (characterDies && !isEnemy)
            {
                StartCoroutine(KillPlayer());
            }
            if (characterDies && isEnemy)
            {
                StartCoroutine(KillEnemy());
            }
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
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);

            var enemy = GetComponent<Enemy>();
            enemy.isAlive = false;
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