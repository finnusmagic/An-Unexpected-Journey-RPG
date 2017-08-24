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

        [Space(10)]
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Projectile projectile;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

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
        [SerializeField] float patrolSpeed = 0;

        float waitTime = 20f;
        float walkTime = 20f;
        float currentWaitTime;
        float currentWalkTime;

        int CurrentWaypointID = 0;
        float reachDistance = 1.5f;
        string pathName;
        Vector3 last_position;
        Vector3 current_position;

        Vector3 MoveToNextPath;

<<<<<<< HEAD
        public enum State { idle, patrolling, attacking, chasing }
        State state = State.patrolling;
=======

        public enum State { idle, patrolling, attacking, chasing, gettingAttacked }
        State state = State.idle;
>>>>>>> parent of ed691a1... NPC / Dialogue / Audio Manager Fix

        public bool underAttack = false;


        public float GetEnemyPatrolSpeed()
        {
            return patrolSpeed;
        }


        void Start()
        {
            playerStatus = FindObjectOfType<PlayerStatusManager>();
            player = FindObjectOfType<PlayerMovement>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            weaponSystem = GetComponent<WeaponSystem>();
            enemyStatus = GetComponent<EnemyStatus>();
            character = GetComponent<Character>();

            if (weaponSystem.currentWeaponConfig.isRanged)
            {
                projectile = weaponSystem.currentWeaponConfig.GetProjectilePrefab().GetComponent<Projectile>();
            }
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            if (distanceToPlayer > chaseRadius && state != State.idle && !underAttack)
            {
                character.isPatrolling = true;
                StopAllCoroutines();
                StartCoroutine(StartPatrolling());
            }
<<<<<<< HEAD

            if (distanceToPlayer > chaseRadius && state != State.patrolling && !underAttack)
=======
        }

        private void CheckForPatrolling()
        {
            if (patrolPath != null)
>>>>>>> parent of ed691a1... NPC / Dialogue / Audio Manager Fix
            {
                character.isPatrolling = false;
                StopAllCoroutines();
               // state = State.idle;
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
<<<<<<< HEAD
                character.isPatrolling = false;
=======
                GetComponent<NavMeshAgent>().speed = chaseSpeed;
>>>>>>> parent of ed691a1... NPC / Dialogue / Audio Manager Fix
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
<<<<<<< HEAD
                character.isPatrolling = false;
=======
                GetComponent<NavMeshAgent>().speed = chaseSpeed;
>>>>>>> parent of ed691a1... NPC / Dialogue / Audio Manager Fix
                StopAllCoroutines();
                StartCoroutine(AttackPlayer());
            }

            if(underAttack)
            {
                character.isPatrolling = false;
                StopAllCoroutines();
                character.SetDestination(player.transform.position);
            }
        }

        void InstantiateEnemyPath()
        {
            last_position = transform.position;
        }

        void PatrolPath()
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
                PatrolPath();

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

                if (!weaponSystem.currentWeaponConfig.isRanged)
                {
                    Invoke("DamagePlayer", .5f);
                }

                if (weaponSystem.currentWeaponConfig.isRanged)
                {
                    StartCoroutine(ShootPlayer());
                }
            }
        }

        IEnumerator ShootPlayer()
        {
            SpawnProjectile();
            yield return new WaitForSeconds(.3f);
        }

        void SpawnProjectile()
        {
            projectile.damageCaused = weaponSystem.currentWeaponConfig.GetAdditionalDamage();
            GameObject newProjectile = Instantiate(weaponSystem.currentWeaponConfig.GetProjectilePrefab(), projectileSocket.transform.position, Quaternion.identity);
            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * weaponSystem.currentWeaponConfig.GetProjectileSpeed();
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