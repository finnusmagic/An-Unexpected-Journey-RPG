using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed;
        [SerializeField] float damageCaused;
        const float DESTROY_DELAY_AFTER_HIT = 0.01f;
        const float DESTROY_DELAY = 2f;

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(damageCaused);
                Destroy(gameObject, DESTROY_DELAY_AFTER_HIT);
            }
            else
            {
                Destroy(gameObject, DESTROY_DELAY);
            }
        }
    }
}
