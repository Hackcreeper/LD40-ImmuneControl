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

        public Material UpgradeTopMaterial;
        public Material UpgradeBodyMaterial;
        public Material UpgradeShootMaterial;
        public Material UpgradeBulletMaterial;

        public Transform HeadShot;
        public Transform Body;

        private Collider enemy;
        private float timer;

        protected override void OnUpdate()
        {
            if (enemy == null)
            {
                enemy = GetNearestEnemy();
            }

            if (enemy == null) return;

            if (Vector3.Distance(enemy.transform.position, transform.position) > Radius)
            {
                enemy = null;
                return;
            }

            if (enemy.GetComponent<Enemy>().IsPulled())
            {
                enemy = null;
                return;
            }

            var lookPos = enemy.transform.position - Head.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + 90, rotation.eulerAngles.z);
            Head.rotation = Quaternion.Slerp(Head.rotation, rotation, Time.deltaTime * 20f);

            timer -= Time.deltaTime;
            if (timer > 0) return;

            GetComponent<Animator>().SetBool("shooting", true);
            StartCoroutine("DisableShooting");

            timer = Delay;
        }

        private Collider GetNearestEnemy()
        {
            return Physics.OverlapSphere(transform.position, Radius)
                .FirstOrDefault(enemy =>
                    enemy.GetComponent<Enemy>() != null && enemy.GetComponent<Enemy>().IsPulled() == false);
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
            bullet.GetComponent<Bullet>().SetSpawner(this);

            if (IsUpgraded())
            {
                bullet.GetComponentInChildren<MeshRenderer>().material = UpgradeBulletMaterial;
            }
            
            GetComponent<AudioSource>().Play();
        }

        protected override void OnUpgrade()
        {
            Delay /= 2;
            Damage *= 2;

            Body.GetComponent<MeshRenderer>().material = UpgradeBodyMaterial;
            Head.GetComponent<MeshRenderer>().material = UpgradeTopMaterial;
            HeadShot.GetComponent<MeshRenderer>().material = UpgradeShootMaterial;
        }
    }
}