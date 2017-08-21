using UnityEngine.Assertions;
using UnityEngine;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float weaponDamage;
        [SerializeField] GameObject projectileSocket = null;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        private static FloatingText popupText;

        public WeaponConfig currentWeaponConfig = null;

        public GameObject target = null;
        GameObject weaponObject;
        Animator animator;
        Character character;
        float lastHitTime;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        private PlayerStatusManager playerStatus;
        private PlayerMovement player;
        private LockTarget lockTarget;

        bool isAttacking = false;
        bool rotatedTowardsEnemy = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            playerStatus = FindObjectOfType<PlayerStatusManager>();
            player = FindObjectOfType<PlayerMovement>();
            lockTarget = GetComponent<LockTarget>();

            PutWeaponInHand(currentWeaponConfig); 

            if (currentWeaponConfig != null)
            weaponDamage = currentWeaponConfig.GetAdditionalDamage();

            InitializeFloatingText();
        }

        public static void InitializeFloatingText()
        {
            if(!popupText)
            popupText = Resources.Load<FloatingText>("Prefabs/Damage Number");
        }

        public void CreateFloatingText(string text, Transform location)
        {
            FloatingText instance = Instantiate(popupText);
            instance.transform.SetParent(target.transform, false);
            instance.SetText(text);
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
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

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            SetAttackAnimation();

            if (currentWeaponConfig.isRanged && target != null)
            {
                if (!isAttacking && !player.IsTargetInRange(targetToAttack) && target != null)
                {
                    character.SetDestination(lockTarget.target.transform.position);
                }
                else if (!isAttacking && player.IsTargetInRange(targetToAttack) && target != null)
                {
                    target = targetToAttack;
                    StartCoroutine("DamageEnemyRanged");

                    CreateFloatingText(playerStatus.CalculateDamage().ToString(), target.transform);
                }
            }

            else if (!currentWeaponConfig.isRanged && target != null)
            {
                character.SetDestination(lockTarget.target.transform.position);

                if (!isAttacking && player.IsTargetInRange(targetToAttack) && target!= null)
                {
                    target = targetToAttack;
                    StartCoroutine("DamageEnemyMeele");

                    CreateFloatingText(playerStatus.CalculateDamage().ToString(), target.transform);
                }
            }
        }

        IEnumerator DamageEnemyMeele()
        {
            player.RotateTowards(target.GetComponent<EnemyAI>());
            isAttacking = true;
            animator.SetTrigger(ATTACK_TRIGGER);
            target.GetComponent<EnemyStatus>().TakeDamage(playerStatus.CalculateDamage());
            yield return new WaitForSeconds(1f);
            isAttacking = false;
        }

        IEnumerator DamageEnemyRanged()
        {
            StartCoroutine("ShootEnemy");
            player.RotateTowards(target.GetComponent<EnemyAI>());
            isAttacking = true;
            animator.SetTrigger(ATTACK_TRIGGER);
            yield return new WaitForSeconds(1f);
            isAttacking = false;
        }

        IEnumerator ShootEnemy()
        {
            yield return new WaitForSeconds(.3f);
            SpawnProjectile();
        }

        public float CalculateDamage()
        {
            return weaponDamage;
        }

        void SpawnProjectile()
        {
            Projectile projectile = currentWeaponConfig.GetProjectilePrefab().GetComponent<Projectile>();
            projectile.damageCaused = playerStatus.CalculateDamage();
            projectile.isPlayer = true;

            if (target != null)
            {
                GameObject newProjectile = Instantiate(currentWeaponConfig.GetProjectilePrefab(), projectileSocket.transform.position, Quaternion.identity);
                Vector3 unitVectorToEnemy = (target.transform.position + aimOffset - projectileSocket.transform.position).normalized;
                newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToEnemy * currentWeaponConfig.GetProjectileSpeed();
            }
        }
    }
}