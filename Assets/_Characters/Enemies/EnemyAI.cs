using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("Combat Setup")]
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float chaseSpeed = 1f;
        float currentWeaponRange;

        [Space(10)]
        [SerializeField] GameObject projectileSocket;

        PlayerMovement player = null;
        Character character;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        float distanceToPlayer;
        float lastTimeHit;

        Character playerCharacter;

        [Header("Patrolling Setup")]

        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waitTime = 2f;
        [SerializeField] float patrolSpeed = 0.5f;
        [SerializeField] float waypointTolerance = 2f;
        int nextWaypointIndex;

        public enum State { idle, patrolling, attacking, chasing, gettingAttacked }
        State state = State.idle;

        public bool gettingAttacked = false;

        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
            character = GetComponent<Character>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            playerCharacter = player.GetComponent<Character>();

            if (character.characterAlive && playerCharacter.characterAlive)
            {
                CheckForPatrolling();
                CheckForGettingAttacked();
                CheckForChasing();
                CheckForAttacking();
            }
            else
            {
                StopAllCoroutines();
            }
        }

        public float GetChaseRadius()
        {
            return chaseRadius;
        }

        private void CheckForPatrolling()
        {
            if (patrolPath != null)
            {
                if (distanceToPlayer > chaseRadius && state != State.patrolling) // Patrolling
                {
                    GetComponent<NavMeshAgent>().speed = patrolSpeed;
                    StopAllCoroutines();
                    StartCoroutine(Patrol());
                }
            }
        }

        private void CheckForGettingAttacked()
        {
            if (gettingAttacked && state != State.gettingAttacked) // Getting Attacked
            {
                EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
                enemyStatus.CheckForTriggerSounds();

                GetComponent<NavMeshAgent>().speed = chaseSpeed;
                StopAllCoroutines();
                StartCoroutine(ReactToAttack());
            }
        }

        private void CheckForChasing()
        {
            if (distanceToPlayer <= chaseRadius && state != State.chasing) //Chasing
            {
                EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
                enemyStatus.CheckForTriggerSounds();

                GetComponent<NavMeshAgent>().speed = chaseSpeed;
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
        }

        private void CheckForAttacking()
        {
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking) //Attacking
            {
                StopAllCoroutines();
                StartCoroutine(AttackPlayer());
            }
        }

        IEnumerator Patrol()
        {
            state = State.patrolling;

            while (true)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(waitTime);
            }
        }

        IEnumerator ReactToAttack()
        {
            state = State.gettingAttacked;

            while (gettingAttacked)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
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
                WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
                weaponSystem.target = player.gameObject;
                weaponSystem.AttackPlayer(player.gameObject);
                yield return new WaitForEndOfFrame();
            }
        }

        void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if(Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
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