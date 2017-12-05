using System.Linq;
using UnityEngine;

namespace LD40.Towers
{
    public class Antibody : Tower
    {
        public LayerMask EnemyMask;
        public int StartHealth = 20;
        public AudioClip StartSound;
        public Material UpgradedMaterial;

        private bool active;
        private bool reachedPosition;
        private bool dead;

        private Transform targetEnemy;
        private float timer;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private float speed = 3f;

        protected override void OnUpdate()
        {
            if (dead) return;

            if (!active)
            {
                if (!CheckEnemy()) return;

                active = true;
                targetEnemy = GetNearestEnemy();
                targetEnemy.GetComponent<Enemy>().Reserve();

                var audioSource = GetComponent<AudioSource>();
                audioSource.clip = StartSound;
                audioSource.Play();
            }
            else
            {
                if (!reachedPosition)
                {
                    if (!targetEnemy)
                    {
                        active = false;
                        return;
                    }

                    transform.position =
                        Vector3.MoveTowards(transform.position, targetEnemy.position, speed * Time.deltaTime);

                    transform.Rotate(new Vector3(0, 0, 20f));

                    if (Vector3.Distance(transform.position, targetEnemy.position) <= 0.3f)
                    {
                        reachedPosition = true;
                    }
                }
                else
                {
                    timer -= Time.deltaTime;
                    transform.Rotate(new Vector3(0, 0, 1f));

                    if (timer >= 0) return;

                    timer = 1f;
                    AttachedEnemies.ForEach(enemy =>
                    {
                        if (enemy == null) return;

                        if (enemy.GetComponent<EntityHealth>().Sub(Damage))
                        {
                            Killed++;
                        }
                    });
                }
            }
        }

        protected override void OnPlace()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            
            var health = gameObject.AddComponent<EntityHealth>();
            health.Health = StartHealth;
            health.FakeDeath = true;
            health.OnDead += (sender, args) =>
            {
                active = false;
                reachedPosition = false;
                dead = true;
                targetEnemy = null;

                gameObject.SetActive(false);

                AttachedEnemies.ForEach(enemy => enemy.InformStickDeath(this));

                if (circle)
                {
                    Destroy(circle.gameObject);
                }
            };

            EnemySpawner.Instance.OnEnd += (sender, args) =>
            {
                if (!this)
                {
                    return;
                }
                
                active = false;
                reachedPosition = false;
                dead = false;
                targetEnemy = null;

                gameObject.SetActive(true);
                transform.position = originalPosition;
                transform.rotation = originalRotation;

                health.Health = StartHealth;

                if (DetailPanel.Instance.IsMe(this))
                {
                    CreateCircleIfNotExists();
                }
            };
        }

        private bool CheckEnemy()
        {
            var elements = Physics.OverlapSphere(transform.position, Radius, EnemyMask);

            return elements.Length != 0 && elements.All(enemy => enemy.GetComponent<Enemy>().IsPulled() == false);
        }

        private Transform GetNearestEnemy()
        {
            return Physics.OverlapSphere(transform.position, Radius, EnemyMask)[0].transform;
        }

        protected override void OnUpgrade()
        {
            StartHealth *= 2;
            GetComponent<EntityHealth>().Health = StartHealth;
            GetComponentInChildren<MeshRenderer>().material = UpgradedMaterial;

            speed *= 2;
        }
    }
}