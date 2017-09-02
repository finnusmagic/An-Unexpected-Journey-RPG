using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] string abilityName;
        [SerializeField] AnimationClip abilityAnimation;
        [SerializeField] float manaCost;
        public float coolDown;
        public float currentCooldown;
        public Image abilityIcon = null;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToattachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToattachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public float GetManaCost()
        {
            return manaCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        public AnimationClip GetAbilityAnimation()
        {
            return abilityAnimation;
        }

        public string GetAbilityName()
        {
            return abilityName;
        }
    }
}