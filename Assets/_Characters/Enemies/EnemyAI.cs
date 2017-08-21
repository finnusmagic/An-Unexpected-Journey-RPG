using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;

        bool isAttacking = false;
        float currentWeaponRange;

        PlayerMovement player = null;
        PlayerStatusManager playerStatus;
        NavMeshAgent agent;
        Animator animator;
        WeaponSystem weaponSystem;
        EnemyStatus enemyStatus;
        Character character;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        float distanceToPlayer;
        float lastTimeHit;

        void Start()
        {
            playerStatus = FindObjectOfType<PlayerStatusManager>();
            player = FindObjectOfType<PlayerMovement>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            weaponSystem = GetComponent<WeaponSystem>();
            enemyStatus = GetComponent<EnemyStatus>();
            character = GetComponent<Character>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            if (distanceToPlayer <= chaseRadius)
            {
                agent.SetDestination(player.transform.position);
            }
            if (distanceToPlayer <= currentWeaponRange && player.GetComponent<PlayerStatusManager>().isAlive)
            {
                AttackPlayer();
            }

        }

        private void SetAttackAnimation()
        {
            if (weaponSystem != null)
            {
                animator = GetComponent<Animator>();
                var animatorOverrideController = character.GetOverrideController();

                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK] = weaponSystem.GetCurrentWeapon().GetAttackAnimClip();
            }
        }

        void AttackPlayer()
        {
            if (Time.time - lastTimeHit > weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits() && enemyStatus.isAlive)
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                lastTimeHit = Time.time;
                Invoke("DamagePlayer", .5f);
            }
        }

        public void DamagePlayer()
        {
            var weaponDamage = weaponSystem.CalculateDamage();
            player.GetComponent<PlayerStatusManager>().DamagePlayer(weaponDamage);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}