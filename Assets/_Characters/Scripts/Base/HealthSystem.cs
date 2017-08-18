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
        [SerializeField] GameObject enemyCanvas = null;
        [Space(10)]
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] AudioClip[] damageSounds = null;
        [SerializeField] AudioClip[] deathSounds = null;
        [SerializeField] float deathVanishSeconds = 2.0f;

        const string DEATH_TRIGGER = "Death";

        float currentHealthPoints;
        Animator animator;
        AudioSource audioSource;
        Character characterMovement;

        GameObject enemyHealth;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();

            currentHealthPoints = maxHealthPoints;

            Vector3 enemyCanvasPosition = new Vector3(0, 2.5f, 0) + transform.position;
            enemyHealth = Instantiate(enemyCanvas, enemyCanvasPosition, transform.rotation);
            enemyHealth.transform.SetParent(transform);

            enemyCanvas.transform.GetChild(1).GetComponent<Image>().fillAmount = healthAsPercentage;
        }

        void Update()
        {
            UpdateHealthBar();
        }

        void UpdateHealthBar()
        {
            enemyCanvas.transform.GetChild(1).GetComponent<Image>().fillAmount = healthAsPercentage;
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