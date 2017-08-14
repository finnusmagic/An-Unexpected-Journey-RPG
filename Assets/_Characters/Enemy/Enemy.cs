using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;

        [SerializeField] float firingPeriodInS = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        [SerializeField] float rotationSpeed = 10f;

        bool isAttacking = false;

        public bool isAlive = true;
        PlayerCombat player = null;

        NavMeshAgent agent;
        Vector3 startPoint;

        void Start()
        {
            player = FindObjectOfType<PlayerCombat>();
            agent = GetComponent<NavMeshAgent>();
            startPoint = transform.position;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= attackRadius && isAlive)
            {
                RotateTowards(player);
            }

            if (distanceToPlayer <= attackRadius && !isAttacking && isAlive)
            {
                isAttacking = true;
                float randomisedDelay = Random.Range(firingPeriodInS - firingPeriodVariation, firingPeriodInS + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay);
            }

            if (distanceToPlayer > attackRadius && isAlive)
            {
                MoveBackToStart();
            }

            if (distanceToPlayer <= chaseRadius && isAlive)
            {
                agent.SetDestination(player.transform.position);
            }
        }

        private void MoveBackToStart()
        {
            isAttacking = false;
            CancelInvoke();
            agent.SetDestination(startPoint);
        }

        private void RotateTowards(PlayerCombat player)
        {
            if (isAlive)
            {
                agent.updateRotation = true;
                Vector3 direction = (player.transform.position - agent.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        void FireProjectile()
        {
            if (isAlive)
            {
                GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
                Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
                projectileComponent.SetDamage(damagePerShot);

                Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
                float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
                newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}