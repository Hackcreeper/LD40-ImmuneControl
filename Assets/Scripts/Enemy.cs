using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace LD40
{
    public class Enemy : MonoBehaviour
    {
        public float Speed = 5f;
        public int Damage = 1;
        public int StartHealth = 10;

        private int currentNode;
        private Vector3? targetPosition;
        private readonly List<Tower> stickyTargets = new List<Tower>();
        private float timer;

        private void Start()
        {
            targetPosition = Route.Instance.GetPosition(currentNode);

            var entityHealth = gameObject.AddComponent<EntityHealth>();
            entityHealth.Health = StartHealth;
            entityHealth.OnDead += (sender, args) =>
            {
                Cells.Instance.Add(Damage);
                EnemySpawner.Instance.KilledEnemy();
                if (stickyTargets.Count > 0)
                {
                    stickyTargets.ForEach(target => target.InformDeath(this));
                }
            };
        }

        private void Update()
        {
            if (stickyTargets.Count > 0)
            {
                timer -= Time.deltaTime;
                if (timer > 0) return;

                timer = 1f;
                stickyTargets.ForEach(target => target.GetComponent<EntityHealth>().Sub(1 * Damage));
                return;
            }

            if (!targetPosition.HasValue)
            {
                Health.Instance.Sub(1 * Damage);
                EnemySpawner.Instance.KilledEnemy();
                Destroy(gameObject);

                return;
            }

            transform.position =
                Vector3.MoveTowards(transform.position, targetPosition.Value, Speed * Time.deltaTime);

            if (!(Vector3.Distance(transform.position, targetPosition.Value) <= 0.1f)) return;

            currentNode++;
            targetPosition = Route.Instance.GetPosition(currentNode);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Tower")) return;

            var target = other.gameObject.GetComponent<Tower>();
            stickyTargets.Add(target);
            target.InformEnemy(this);
        }

        public void InformStickDeath(Tower tower)
        {
            stickyTargets.Remove(tower);
        }
    }
}