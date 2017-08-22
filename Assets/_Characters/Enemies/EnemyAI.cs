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
        float currentWeaponRange;

        [Space(10)]
        [SerializeField] GameObject projectileSocket;

        PlayerMovement player = null;
        WeaponSystem weaponSystem;
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

        public enum State { idle, patrolling, attacking, chasing }

        State state = State.patrolling;

        public bool underAttack = false;


        public float GetEnemyPatrolSpeed()
        {
            return patrolSpeed;
        }

        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            CheckForIdle();
            CheckForPatrolling();
            CheckForChasing();
            CheckForAttacking();
            CheckIfUnderAttack();
        }

        private void CheckIfUnderAttack()
        {
            if (underAttack) //Under Attack
            {
                character.isPatrolling = false;
                StopAllCoroutines();
                character.SetDestination(player.transform.position);
            }
        }

        private void CheckForAttacking()
        {
            if (distanceToPlayer <= currentWeaponRange) //Attacking
            {
                character.isPatrolling = false;
                StopAllCoroutines();
                StartCoroutine(AttackPlayer());
            }
        }

        private void CheckForChasing()
        {
            if (distanceToPlayer <= chaseRadius) //Chasing
            {
                character.isPatrolling = false;
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
        }

        private void CheckForPatrolling()
        {
            if (distanceToPlayer > chaseRadius && state != State.idle && !underAttack && state != State.attacking) // Patrolling
            {
                character.isPatrolling = true;
                StopAllCoroutines();
                StartCoroutine(StartPatrolling());
            }
        }

        private void CheckForIdle()
        {
            if (distanceToPlayer > chaseRadius && state != State.patrolling && state != State.idle && !underAttack) //Idle
            {
                character.isPatrolling = false;
                StopAllCoroutines();
                state = State.idle;
            }
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
                weaponSystem.target = player.gameObject;
                weaponSystem.AttackTarget(player.gameObject);
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

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}