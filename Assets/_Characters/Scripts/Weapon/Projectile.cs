using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        public float damageCaused;
        public bool isPlayer = false;

        const float DESTROY_DELAY_AFTER_HIT = 0.01f;
        const float DESTROY_DELAY = 2f;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<EnemyStatus>().TakeDamage(damageCaused);
                Destroy(gameObject, DESTROY_DELAY_AFTER_HIT);
            }
            else
            {
                Destroy(gameObject, DESTROY_DELAY);
            }

            if (collider.gameObject.tag == "Player" && !isPlayer)
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
