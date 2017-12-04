using LD40.Towers;
using UnityEngine;

namespace LD40
{
    public class Bullet : MonoBehaviour
    {
        public float Speed = 20f;

        private Tower Spawner;
        private float timer = 5f;

        private void Update()
        {
            transform.Translate(new Vector3(
                -Speed * Time.deltaTime, 0, 0
            ));

            timer -= Time.deltaTime;
            if (timer > 0) return;

            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (other.gameObject.GetComponent<EntityHealth>().Sub(Spawner.Damage))
                {
                    Spawner.Killed++;
                }
                Destroy(gameObject);
            }
        }

        public void SetSpawner(Tower spawner)
        {
            Spawner = spawner;
        }
    }
}