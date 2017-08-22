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
        WeaponSystem weaponSystem;

        CameraRaycaster cameraRaycaster;
        Character character;
        EnemyAI enemy;

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
    }
}