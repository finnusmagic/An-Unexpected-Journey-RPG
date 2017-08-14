using UnityEngine.Assertions;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class WeaponConfig : MonoBehaviour
    {
        [SerializeField] Weapon currentWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle = null;
        [SerializeField] AudioClip attackSound, critSound;
        AudioSource audioSource;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        Animator animator = null;
        SpecialAbilities abilities;

        float lastHitTime = 0;
        GameObject weaponObject;
        PlayerCombat playerCombat;

        void Start()
        {
            playerCombat = GetComponent<PlayerCombat>();
            abilities = GetComponent<SpecialAbilities>();

            PutWeaponInHand(currentWeaponConfig); 
            SetAttackAnimation();

            audioSource = GetComponent<AudioSource>();
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
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
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;

            float damageBeforeCritical = playerCombat.baseDamage + currentWeaponConfig.GetAdditionalDamage();

            if (isCriticalHit)
            {
                criticalHitParticle.Play();
                audioSource.clip = critSound;
                audioSource.Play();
                return damageBeforeCritical * criticalHitMultiplier;
            }
            else
            {
                audioSource.clip = attackSound;
                audioSource.Play();
                return damageBeforeCritical;
            }
        }

        public bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();
        }
    }
}