using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Animator")] [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        const string DEATH_TRIGGER = "Death";

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.03f;

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = .7f;
        public float animationSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float navMeshAgentBaseOffset = 0f;
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;
        [SerializeField] float navMeshAgentRadius = 0.3f;
        [SerializeField] float navMeshAgentHeight = 1.8f;

        [Space(10)] public bool characterAlive = true;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody rigidBody;
        float turnAmount;
        float forwardAmount;
        private static FloatingText popupText;


        void Awake()
        {
            AddRequiredComponents();
        }

        public float GetTurnSpeed()
        {
            return movingTurnSpeed;
        }

        public float GetMoveSpeed()
        {
            return moveSpeedMultiplier;
        }

        public float GetAnimSpeedMultiplier()
        {
            return animator.speed;
        }


        private void AddRequiredComponents()
        {
            InitializeFloatingText();

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animator.avatar = characterAvatar;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.radius = navMeshAgentRadius;
            navMeshAgent.height = navMeshAgentHeight;
            navMeshAgent.baseOffset = navMeshAgentBaseOffset;
        }

        void Update()
        {
            if (navMeshAgent != null && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }

            if (!characterAlive)
            {
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.updateRotation = false;
                Move(Vector3.zero);
            }
        }

        public IEnumerator KillCharacter()
        {
            characterAlive = false;

            if (GetComponent<EnemyStatsManager>() != null)
            {
                FindObjectOfType<LevelUpSystem>().AddXP(GetComponent<EnemyStatsManager>().GetEnemyXP());
            }

            if (GetComponent<PlayerMovement>() != null)
            {
                GetComponent<PlayerMovement>().canMove = false;
            }

            var agent = GetComponent<NavMeshAgent>();
            agent.velocity = Vector3.zero;

            GetComponent<Animator>().SetTrigger(DEATH_TRIGGER);

            yield return new WaitForSecondsRealtime(2f);

            if (GetComponent<PlayerMovement>() != null)
            {
                SceneManager.LoadScene("02_Start_Game_Scene");
            }

            StopAllCoroutines();
            Destroy(gameObject);
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }
        }

        public static void InitializeFloatingText()
        {
            if (!popupText)
                popupText = Resources.Load<FloatingText>("Prefabs/Damage Number");
        }

        public void CreateFloatingText(string text, Transform location)
        {
            FloatingText instance = Instantiate(popupText);
            instance.transform.SetParent(transform, false);
            instance.SetText(text);
        }
    }
}