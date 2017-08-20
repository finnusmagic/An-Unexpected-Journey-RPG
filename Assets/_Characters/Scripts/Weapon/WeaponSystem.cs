using UnityEngine.Assertions;
using UnityEngine;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float weaponDamage;
        public WeaponConfig currentWeaponConfig = null;

        public GameObject target = null;
        GameObject weaponObject;
        Animator animator;
        Character character;
        float lastHitTime;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        private CameraRaycaster cameraRaycaster;

        bool isAttacking = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();

            PutWeaponInHand(currentWeaponConfig); 
            SetAttackAnimation();

            if (currentWeaponConfig != null)
            weaponDamage = currentWeaponConfig.GetAdditionalDamage();
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
            if (!isAttacking)
            {
                target = targetToAttack;
                StartCoroutine("DamageEnemy");
            }
        }

        IEnumerator DamageEnemy()
        {
            isAttacking = true;
            animator.SetTrigger(ATTACK_TRIGGER);
            target.GetComponent<EnemyStatus>().TakeDamage(5);
            yield return new WaitForSeconds(1f);
            isAttacking = false;
        }


        public float CalculateDamage()
        {
            return weaponDamage;
        }
    }
}