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

        Character character;
        EnemyAI enemy;

        public float walkSpeed = 1;
        public float runSpeed = 2;

        public float turnSmoothTime = 0.2f;
        float turnSmoothVelocity;

        public float speedSmoothTime = 0.1f;
        float speedSmoothVelocity;
        float currentSpeed;

        public bool canMove = true;

        Animator animator;
        Transform cameraT;

        void Start()
        {
            cameraT = Camera.main.transform;
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
            RegisterForMouseEvents();

            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 inputDir = input.normalized;

            if (inputDir != Vector2.zero)
            {
                turnSmoothTime = 0.1f;
            }
            else
            {
                turnSmoothTime = 0.5f;
            }

            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

            bool walking = Input.GetKey(KeyCode.LeftShift);
            float targetSpeed = ((walking) ? walkSpeed : runSpeed) * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

            float animationSpeedPercent = ((walking) ? 1f : 2f) *inputDir.magnitude;
            animator.SetFloat("Forward", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Alpha1) && character.characterAlive)
            {
                if (enemy != null)
                    weaponSystem.AttackEnemy(enemy.gameObject);
            }
        }


        private void RegisterForMouseEvents()
        {
            CameraRaycaster cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                LockTarget lockTarget = GetComponent<LockTarget>();
                lockTarget.target = null;
            }
        }
        void OnMouseOverEnemy(EnemyAI enemyToSet)
        {
            enemy = enemyToSet;

            if (Input.GetMouseButton(0))
            {
                LockTarget lockTarget = GetComponent<LockTarget>();
                lockTarget.targetPanel.SetActive(true);
                lockTarget.target = enemy;
            }
        }
    }
}