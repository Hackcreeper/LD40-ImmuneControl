using UnityEngine;

namespace LD40.Enemies
{
    public class Worm : Enemy
    {
        public GameObject SplitPrefab;
        
        protected override void OnUpdate()
        {
            if (targetPosition.HasValue)
            {
                var lookPos = targetPosition.Value - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20f);                
            }
        }

        protected override void OnDeath()
        {
            if (SplitPrefab)
            {
                var ax = Random.Range(-1f, 1f);
                var az = Random.Range(-1f, 1f);
                
                var a = Instantiate(SplitPrefab);
                a.transform.position = transform.position + new Vector3(ax, 0, az);
                a.transform.rotation = transform.rotation;
                a.GetComponent<Enemy>().SetNode(currentNode);
                EnemySpawner.Instance.IncreaseEnemiesLeft();
                
                var bx = Random.Range(-1f, 1f);
                var bz = Random.Range(-1f, 1f);
                
                var b = Instantiate(SplitPrefab);
                b.transform.position = transform.position - new Vector3(ax, 0, az);
                b.transform.rotation = transform.rotation;
                b.GetComponent<Enemy>().SetNode(currentNode);
                EnemySpawner.Instance.IncreaseEnemiesLeft();
            }
        }
    }
}