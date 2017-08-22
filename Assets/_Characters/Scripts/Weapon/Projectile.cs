using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        public float damageCaused;

        const float DESTROY_DELAY_AFTER_HIT = 0.01f;
        const float DESTROY_DELAY = 2f;

        public bool targetIsPlayer = false;
        public bool targetIsEnemy = false;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Enemy" && targetIsEnemy)
            {
                collider.gameObject.GetComponent<EnemyStatus>().TakeDamage(damageCaused);
                Destroy(gameObject, DESTROY_DELAY_AFTER_HIT);
            }

            else if (collider.gameObject.tag == "Player" && targetIsPlayer)
            {
                collider.gameObject.GetComponent<PlayerStatusManager>().DamagePlayer(damageCaused);
                Destroy(gameObject, DESTROY_DELAY_AFTER_HIT);
            }

            else
            {
                Destroy(gameObject, DESTROY_DELAY);
            }
        }
    }
}
