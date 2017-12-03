using System.Collections;
using System.Linq;
using UnityEngine;

namespace LD40.Towers
{
    public class BloodCell : Tower
    {
        public Transform Head;
        public float Delay = 1f;
        public Transform ShotSpawn;
        public GameObject BulletPrefab;

        private Collider enemy;
        private float timer;

        protected override void OnUpdate()
        {
            if (enemy == null)
            {
                enemy = GetNearestEnemy();
            }

            if (enemy != null)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) > Radius)
                {
                    enemy = null;
                    return;
                }
                
                var lookPos = enemy.transform.position - Head.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                rotation = Quaternion.Euler(-90, rotation.eulerAngles.y, rotation.eulerAngles.z + 90);
                Head.rotation = Quaternion.Slerp(Head.rotation, rotation, Time.deltaTime * 10f);

                timer -= Time.deltaTime;
                if (timer > 0) return;

                GetComponent<Animator>().SetBool("shooting", true);
                StartCoroutine("DisableShooting");
                
                timer = Delay;
            }
        }

        private Collider GetNearestEnemy()
        {
            return Physics.OverlapSphere(transform.position, Radius)
                .FirstOrDefault(enemy => enemy.GetComponent<Enemy>() != null);
        }

        private IEnumerator DisableShooting()
        {
            yield return new WaitForEndOfFrame();
            
            GetComponent<Animator>().SetBool("shooting", false);
        }

        public void Shoot()
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = ShotSpawn.position;
            bullet.transform.rotation = Head.transform.rotation;
        }
    }
}