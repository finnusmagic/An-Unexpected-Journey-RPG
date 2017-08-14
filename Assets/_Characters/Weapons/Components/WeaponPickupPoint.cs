using UnityEngine;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;
        [SerializeField] ParticleSystem pickupParticleEffect;

        AudioSource audioSource;
        bool pickedUp = false;

        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
            if (pickedUp)
            {
                foreach (Transform child in transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
               DestroyImmediate(child.gameObject);
            } 
        }

        private void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        void OnTriggerEnter(Collider collider)
        {
            if (!pickedUp && collider.gameObject.tag == "Player")
            {
                FindObjectOfType<WeaponConfig>().PutWeaponInHand(weaponConfig);
                PlayPickupSound();
                PlayParticleEffect();
                pickedUp = true;
            }
        }

        private void PlayParticleEffect()
        {
            pickupParticleEffect = GetComponent<ParticleSystem>();
            pickupParticleEffect.Play();
        }

        private void PlayPickupSound()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(pickUpSFX);
        }
    }
}
