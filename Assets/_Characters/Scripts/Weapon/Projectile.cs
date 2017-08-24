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

        bool hit = false;

        private void Update()
        {
            transform.LookAt(transform.position - GetComponent<Rigidbody>().velocity);
            transform.Rotate(90, 180, 0);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (hit == false)
            {
                if (collider.gameObject.tag == "Enemy" && targetIsEnemy)
                {
                    collider.gameObject.GetComponent<EnemyStatus>().TakeDamage(damageCaused);
                    Destroy(gameObject, DESTROY_DELAY_AFTER_HIT);
                    hit = true;
                }

                else if (collider.gameObject.tag == "Player" && targetIsPlayer)
                {
                    collider.gameObject.GetComponent<PlayerStatusManager>().DamagePlayer(damageCaused);
                    Destroy(gameObject, DESTROY_DELAY_AFTER_HIT);
                    hit = true;
                }

                else
                {
                    Destroy(gameObject, DESTROY_DELAY);
                }
            }
        }
    }
}
