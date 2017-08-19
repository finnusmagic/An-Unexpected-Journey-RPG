using UnityEngine.Assertions;
using UnityEngine;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

       // GameObject target;
        GameObject weaponObject;
        Animator animator;
        Character character;
        float lastHitTime;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        public int currentWeaponID;

        void Start()
        {
            character = GetComponent<Character>();

            currentWeaponID = -1;

            PutWeaponInHand(currentWeaponConfig); 
            SetAttackAnimation();
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            if (currentWeaponConfig != null)
            {
                currentWeaponID = currentWeaponConfig.ID;
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
           // target = targetToAttack;
            // use a repeat attack co-routine
        }

        public void AttackTarget(HealthSystem target)
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                DamageTarget(target);
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        public void DamageTarget(HealthSystem targetHealth)
        {
            targetHealth.TakeDamage(CalculateDamage());
        }

        private float CalculateDamage()
        {
            return  baseDamage;
        }
    }
}