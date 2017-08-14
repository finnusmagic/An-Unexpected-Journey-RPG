using UnityEngine;
using RPG.CameraUI;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class PlayerCombat : MonoBehaviour
    {
        public float baseDamage = 10f;
        [SerializeField] float rotationSpeed = 10f;

        SpecialAbilities abilities;
        WeaponConfig weaponConfig;
        CameraRaycaster cameraRaycaster = null;
        Character character;
        Enemy enemy = null;
        HealthSystem enemyHealth = null;
        NavMeshAgent agent = null;

        void Start()
        {
            abilities = GetComponent<SpecialAbilities>();
            character = GetComponent<Character>();
            weaponConfig = GetComponent<WeaponConfig>();
            RegisterForMouseEvents();
        }

        void Update()
        {
            ScanForAbilityKeyDown();
        }

        void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDesination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            enemy = enemyToSet;
            enemyHealth = enemyToSet.GetComponent<HealthSystem>();

            if (Input.GetMouseButton(0) && weaponConfig.IsTargetInRange(enemy.gameObject))
            {
                weaponConfig.AttackTarget(enemyHealth);
                RotateTowards(enemy);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }

        private void RotateTowards(Enemy enemy)
        {
            agent = character.GetComponent<NavMeshAgent>();
            agent.updateRotation = true;
            Vector3 direction = (enemy.transform.position - agent.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            agent.updateRotation = false;
        }
    }
}