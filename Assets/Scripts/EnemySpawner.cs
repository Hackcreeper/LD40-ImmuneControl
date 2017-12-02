using UnityEngine;

namespace LD40
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject EnemyPrefab;

        public Transform SpawnPoint;

        private float timer;

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer > 0)
            {
                return;
            }

            timer = 2f;

            Instantiate(EnemyPrefab).transform.position = SpawnPoint.position;
        }
    }
}