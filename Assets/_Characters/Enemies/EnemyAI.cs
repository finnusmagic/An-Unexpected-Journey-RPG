using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("Combat Setup")]
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

        [Header("Patrolling Setup")]
        public EditorPathScript PathToFollow;
        public float patrolSpeed = 0;
        private float waitTime = 20f;
        private float walkTime = 20f;
        private float currentWaitTime;
        private float currentWalkTime;

        public int CurrentWaypointID = 0;
        private float reachDistance = 1.5f;
        public string pathName;
        Vector3 last_position;
        Vector3 current_position;

        Vector3 MoveToNextPath;


        public enum State { idle, patrolling, attacking, chasing }
        State state = State.patrolling;

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

            if (distanceToPlayer > chaseRadius && state != State.idle)
            {
                character.isPatrolling = true;
                StopAllCoroutines();
                StartCoroutine(StartPatrolling());
            }

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                character.isPatrolling = false;
                StopAllCoroutines();
               // state = State.idle;
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                character.isPatrolling = false;
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                character.isPatrolling = false;
                StopAllCoroutines();
                StartCoroutine(AttackPlayer());
            }
        }

        void InstantiateEnemyPath()
        {
            last_position = transform.position;
        }

        void PatrollPath()
        {
            float distance = Vector3.Distance(PathToFollow.path_objs[CurrentWaypointID].position, transform.position);

            var rotation = Quaternion.LookRotation(PathToFollow.path_objs[CurrentWaypointID].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1f);

            if (distance <= reachDistance)
            {
                CurrentWaypointID++;
            }
            if (CurrentWaypointID >= PathToFollow.path_objs.Count)
            {
                CurrentWaypointID = 0;
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer <= chaseRadius)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator AttackPlayer()
        {
            state = State.attacking;
            while (distanceToPlayer <= currentWeaponRange)
            {
                PerformAttack();
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator StartPatrolling()
        {
            state = State.patrolling;

            while (distanceToPlayer >= chaseRadius)
            {
                PatrollPath();

                if (currentWalkTime <= walkTime)
                {
                    character.SetDestination(PathToFollow.path_objs[CurrentWaypointID].position);
                    currentWalkTime += Time.deltaTime;
                }
                if (currentWalkTime >= walkTime)
                {
                    currentWaitTime += Time.deltaTime;
                }
                if (currentWaitTime >= waitTime)
                {
                    currentWalkTime = Random.Range(5, walkTime);
                    currentWaitTime = Random.Range (5, waitTime);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void SetAttackAnimation()
        {
            if (weaponSystem != null)
            {

                var animatorOverrideController = character.GetOverrideController();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK] = weaponSystem.GetCurrentWeapon().GetAttackAnimClip();
            }
        }

        void PerformAttack()
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