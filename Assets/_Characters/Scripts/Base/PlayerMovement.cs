using UnityEngine;
using RPG.CameraUI;
using UnityEngine.AI;
using RPG.Database;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 10f;

        WeaponSystem weaponSystem;

        CameraRaycaster cameraRaycaster;
        Character character;
        EnemyAI enemy;
        NavMeshAgent agent;

        LockTarget lockTarget;

        public bool canMove = true;

        void Start()
        {
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
            lockTarget = GetComponent<LockTarget>();
            RegisterForMouseEvents();
        }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0) && canMove)
            {
                character.SetDestination(destination);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (enemy != null)
                weaponSystem.AttackTarget(enemy.gameObject);
            }
        }
        void OnMouseOverEnemy(EnemyAI enemyToSet)
        {
            enemy = enemyToSet;

            if (Input.GetMouseButton(0))
            {
                lockTarget.targetPanel.SetActive(true);
                lockTarget.target = enemy;
            }
        }

        public bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        public void RotateTowards(EnemyAI enemy)
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