using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD40
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        public GameObject GreenVirusPrefab;
        public GameObject BlueVirusPrefab;
        public GameObject PurpleVirusPrefab;
        public GameObject SmallpoxPrefab;
        public GameObject InfestedAntibodyPrefab;

        public Transform SpawnPoint;
        public Button waveButton;
        public Text waveText;
        public GameObject WarningText;
        public GameObject TowersPanel;

        public event EventHandler<EventArgs> OnEnd;

        private const int EnemyGreenVirus = 1;
        private const int EnemyBlueVirus = 2;
        private const int EnemyPurpleVirus = 3;
        private const int EnemySmallpox = 4;
        private const int EnemyInfestedAntibody = 5;

        private float timer;
        private readonly List<int[]> waves = new List<int[]>();

        private int wave = -1;
        private int enemiesLeft;
        private int spawnedEnemy;

        private void Awake()
        {
            Instance = this;

            // WAVE 1
            waves.Add(new[]
            {
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus
            });

            // WAVE 2
            waves.Add(new[]
            {
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyBlueVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus
            });

            // WAVE 3
            waves.Add(new[]
            {
                EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus,
                EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyPurpleVirus
            });

            // WAVE 4
            waves.Add(new[]
            {
                EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyPurpleVirus, EnemyBlueVirus, EnemyBlueVirus,
                EnemyPurpleVirus, EnemyPurpleVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus, EnemyBlueVirus,
                EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus
            });

            // WAVE 5
            waves.Add(new[]
            {
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemySmallpox
            });

            // WAVE 6
            waves.Add(new[]
            {
                EnemyPurpleVirus, EnemyPurpleVirus, EnemySmallpox, EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus,
                EnemySmallpox, EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus, EnemySmallpox, EnemyPurpleVirus,
                EnemyPurpleVirus, EnemyPurpleVirus, EnemySmallpox
            });

            waveText.text = string.Format("Wave 0 / {0}", waves.Count);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Time.timeScale /= 2;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Time.timeScale *= 2;
            }

            if (enemiesLeft <= 0)
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
                case EnemyGreenVirus:
                    return GreenVirusPrefab;

                case EnemyBlueVirus:
                    return BlueVirusPrefab;

                case EnemyPurpleVirus:
                    return PurpleVirusPrefab;

                case EnemySmallpox:
                    return SmallpoxPrefab;

                case EnemyInfestedAntibody:
                    return InfestedAntibodyPrefab;

                default:
                    return null;
            }
        }

        public void KilledEnemy()
        {
            enemiesLeft--;

            if (enemiesLeft < 0) enemiesLeft = 0;
            if (enemiesLeft > 0) return;

            WarningText.SetActive(false);
            TowersPanel.SetActive(true);

            if (OnEnd != null)
            {
                OnEnd.Invoke(this, EventArgs.Empty);
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void StartWave()
        {
            if (enemiesLeft > 0) return;

            wave++;
            enemiesLeft = waves[wave].Length;
            spawnedEnemy = 0;

            waveText.text = string.Format("Wave {0} / {1}", wave + 1, waves.Count);

            Debug.Log(string.Format("Starting wave {0}", wave + 1));

            WarningText.SetActive(true);
            TowersPanel.SetActive(false);
            TowerPlacement.Instance.Cancel();
        }

        public void IncreaseEnemiesLeft()
        {
            enemiesLeft++;
        }
    }
}