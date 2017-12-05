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
                // x - 1
                // x + 1
                var a = Instantiate(SplitPrefab);
                a.transform.position = transform.position + new Vector3(1, 0, 0);
                a.transform.rotation = transform.rotation;
                a.GetComponent<Enemy>().SetNode(currentNode);
                EnemySpawner.Instance.IncreaseEnemiesLeft();
                
                var b = Instantiate(SplitPrefab);
                b.transform.position = transform.position - new Vector3(1, 0, 0);
                b.transform.rotation = transform.rotation;
                b.GetComponent<Enemy>().SetNode(currentNode);
                EnemySpawner.Instance.IncreaseEnemiesLeft();
            }
        }
    }
}