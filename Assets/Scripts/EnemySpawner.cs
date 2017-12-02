using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD40
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }
        
        public GameObject ExamplePrefab;
        public Transform SpawnPoint;
        public Button waveButton;
        public Text waveText;

        private const int EnemyExample = 1;

        private float timer;
        private readonly List<int[]> waves = new List<int[]>();
        
        private int wave = -1;
        private int enemiesLeft;
        private int spawnedEnemy;

        private void Start()
        {
            Instance = this;
            
            waves.Add(new[]
            {
                EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample,
                EnemyExample, EnemyExample, EnemyExample
            });

            waves.Add(new[]
            {
                EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample,
                EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample,
                EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample,
                EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample, EnemyExample
            });
            
            waveText.text = string.Format("Wave 0 / {0}", waves.Count);
        }
        
        private void Update()
        {
            if (enemiesLeft == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartWave();
                }
                
                waveButton.interactable = true;
                return;
            }

            waveButton.interactable = false;
            
            timer -= Time.deltaTime;
            if (timer > 0) return;

            timer = 1f;

            if (spawnedEnemy >= waves[wave].Length) return;
            
            var enemy = waves[wave][spawnedEnemy];
            var prefab = GetPrefab(enemy);

            Instantiate(prefab).transform.position = SpawnPoint.position;
            spawnedEnemy++;
        }

        private GameObject GetPrefab(int type)
        {
            switch (type)
            {
                case EnemyExample:
                    return ExamplePrefab;

                default:
                    return null;
            }
        }

        public void KilledEnemy()
        {
            enemiesLeft--;
        }

        public void StartWave()
        {
            if (enemiesLeft != 0) return;
            
            wave++;
            enemiesLeft = waves[wave].Length;
            spawnedEnemy = 0;

            waveText.text = string.Format("Wave {0} / {1}", wave+1, waves.Count);
            
            Debug.Log(string.Format("Starting wave {0}", wave+1));
        }
    }
}