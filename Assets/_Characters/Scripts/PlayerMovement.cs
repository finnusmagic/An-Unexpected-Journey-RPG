using UnityEngine;
using RPG.CameraUI;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 10f;

        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        CameraRaycaster cameraRaycaster;
        Character character;
        Enemy enemy;
        NavMeshAgent agent;

        void Start()
        {
            abilities = GetComponent<SpecialAbilities>();
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();

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

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
                RotateTowards(enemy);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }

        public bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
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