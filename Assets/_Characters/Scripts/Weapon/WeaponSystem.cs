using UnityEngine.Assertions;
using UnityEngine;
using RPG.CameraUI;
using System.Collections;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float weaponDamage;
        [SerializeField] GameObject projectileSocket = null;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        public WeaponConfig currentWeaponConfig = null;

        public GameObject target = null;
        GameObject weaponObject;
        Animator animator;
        Character character;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        public bool isAttacking = false;

        private AudioManager audioManager;

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            if (currentWeaponConfig != null)
            PutWeaponInHand(currentWeaponConfig);

            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("No AudioManager found in the Scene.");
            }
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        private void SetAttackAnimation()
        {
            if (currentWeaponConfig != null)
            {
                animator = GetComponent<Animator>();
                var animatorOverrideController = character.GetOverrideController();

                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
            }
        }

        public void AttackPlayer(GameObject targetToAttack)
        {
            if (character.characterAlive && currentWeaponConfig != null)
            {
                SetAttackAnimation();

                if (currentWeaponConfig.isRanged && target != null)
                {
                    if (!isAttacking && !IsTargetInRange(targetToAttack) && target != null)
                    {
                        character.SetDestination(target.transform.position);
                    }
                    else if (!isAttacking && IsTargetInRange(targetToAttack) && target != null)
                    {
                        target = targetToAttack;
                        StartCoroutine(DamageTargetRanged());
                    }
                }

                else if (!currentWeaponConfig.isRanged && target != null)
                {
                    character.SetDestination(target.transform.position);

                    if (!isAttacking && IsTargetInRange(targetToAttack) && target != null)
                    {
                        target = targetToAttack;
                        StartCoroutine(DamageTargetMeele());
                    }
                }
            }
            else
            {
                StopAllCoroutines();
            }
        }

        public void AttackEnemy(GameObject targetToAttack)
        {
            float attackTimer = currentWeaponConfig.GetMinTimeBetweenHits();

            if (currentWeaponConfig.GetMinTimeBetweenHits() == attackTimer)
            {

                if (character.characterAlive && currentWeaponConfig != null)
                {
                    SetAttackAnimation();

                    if (currentWeaponConfig.isRanged && target != null)
                    {
                        if (!isAttacking && IsTargetInRange(targetToAttack) && target != null)
                        {
                            target = targetToAttack;
                            StartCoroutine(DamageTargetRanged());
                        }
                    }

                    else if (!currentWeaponConfig.isRanged && target != null)
                    {
                        if (!isAttacking && IsTargetInRange(targetToAttack) && target != null)
                        {
                            target = targetToAttack;
                            StartCoroutine(DamageTargetMeele());
                        }
                    }
                }
                else
                {
                    StopAllCoroutines();
                }
            }
        }

        IEnumerator DamageTargetMeele()
        {
            audioManager.PlaySound(currentWeaponConfig.soundName);

            isAttacking = true;
            animator.SetTrigger(ATTACK_TRIGGER);
            if (target.GetComponent<EnemyStatus>() != null)
            {
                target.GetComponent<EnemyStatus>().TakeDamage(FindObjectOfType<PlayerStatusManager>().CalculateDamage());
            }
            else if (target.GetComponent<PlayerStatusManager>() != null)
            {
                target.GetComponent<PlayerStatusManager>().DamagePlayer(currentWeaponConfig.GetAdditionalDamage());
            }
            yield return new WaitForSeconds(currentWeaponConfig.GetMinTimeBetweenHits());
            isAttacking = false;
        }

        IEnumerator DamageTargetRanged()
        {
            audioManager.PlaySound(currentWeaponConfig.soundName);

            transform.LookAt(target.transform);
            StartCoroutine(ShootTarget());
            isAttacking = true;
            animator.SetTrigger(ATTACK_TRIGGER);
            yield return new WaitForSeconds(currentWeaponConfig.GetMinTimeBetweenHits());
            isAttacking = false;
        }

        IEnumerator ShootTarget()
        {
            yield return new WaitForSeconds(.3f);
            SpawnProjectile();
        }

        void SpawnProjectile()
        {
            Projectile projectile = currentWeaponConfig.GetProjectilePrefab().GetComponent<Projectile>();

            if (GetComponent<EnemyAI>() != null) // check if shooter in enemy
            {
                projectile.targetIsPlayer = true;
                projectile.damageCaused = currentWeaponConfig.GetAdditionalDamage();
            }        

            if (GetComponent<PlayerMovement>() != null) // check if shooter is player
            {
                projectile.targetIsEnemy = true;
                projectile.damageCaused = FindObjectOfType<PlayerStatusManager>().CalculateDamage();
            }

            if (target != null)
            {
                GameObject newProjectile = Instantiate(currentWeaponConfig.GetProjectilePrefab(), projectileSocket.transform.position, Quaternion.identity);
                Vector3 unitVectorToEnemy = (target.transform.position + aimOffset - projectileSocket.transform.position).normalized;
                newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToEnemy * currentWeaponConfig.GetProjectileSpeed();
            }
        }

        public bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= GetCurrentWeapon().GetMaxAttackRange();
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            if (currentWeaponConfig != null)
            {
                currentWeaponConfig = weaponToUse;
                var weaponPrefab = weaponToUse.GetWeaponPrefab();
                GameObject dominantHand = RequestDominantHand();
                Destroy(weaponObject);
                weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
                weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
                weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
            }
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }
    }
}