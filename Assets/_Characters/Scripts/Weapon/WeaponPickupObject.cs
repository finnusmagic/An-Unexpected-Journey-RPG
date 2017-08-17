using System;
using UnityEngine;

namespace RPG.Characters
{
    public class WeaponPickupObject : MonoBehaviour
    {
        Vector3 colliderCenter = new Vector3(0, 1.00f, 0);
        [SerializeField]float colliderRadius = 0.3f;
        float colliderHeight = 2.00f;

        WeaponConfig weaponConfig;
        AudioClip pickUpSFX;
        GameObject pickupParticleEffect;

        AudioSource audioSource;

        bool pickedUp = false;
        float  pickedUpTimer;

        private void Start()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;
            capsuleCollider.isTrigger = true;

            audioSource = gameObject.AddComponent<AudioSource>();
            pickUpSFX = Resources.Load<AudioClip>("Sounds/Interaction/weapon_pickup_sound");

            pickedUpTimer = pickUpSFX.length;

            pickupParticleEffect = Resources.Load<GameObject>("Particle Effects/Interaction/weapon_pickup_particles");

            weaponConfig = Resources.Load<WeaponConfig>("Weapons/" + gameObject.name);
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                InstantiateWeapon();
            }
            if (pickedUp)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                pickedUpTimer -= Time.deltaTime;

                if (pickedUpTimer <= 0)
                {
                    Destroy(gameObject);
                }
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
                FindObjectOfType<WeaponSystem>().PutWeaponInHand(weaponConfig);
                PlayPickupSound();
                PlayParticleEffect();
                pickedUp = true;
            }
        }

        private void PlayParticleEffect()
        {
            Instantiate(pickupParticleEffect, transform);
            pickupParticleEffect.GetComponent<ParticleSystem>().Play();
        }

        private void PlayPickupSound()
        {
            audioSource.PlayOneShot(pickUpSFX);
        }
    }
}
