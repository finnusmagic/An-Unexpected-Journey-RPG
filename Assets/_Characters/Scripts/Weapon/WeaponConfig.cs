using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

        [SerializeField] float attackDelay = 0.5f;
        [SerializeField] float minTimeBetweenHits = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [Space(10)]
        public bool isRanged;
        [Space(10)]
        public bool leftHanded;
        public bool rightHanded;
        [Space(10)]
        [SerializeField] GameObject projectile = null;
        [SerializeField] float projectileSpeed = 10f;
        [Space(10)]
        public string soundName;

        public float GetAttackDelay()
        {
            return attackDelay;
        }

        public float GetMinTimeBetweenHits()
        {
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        public GameObject GetProjectilePrefab()
        {
            return projectile;
        }

        public float GetProjectileSpeed()
        {
            return projectileSpeed;
        }

        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
