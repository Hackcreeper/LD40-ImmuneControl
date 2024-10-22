﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public GameObject InfestedMacrophagePrefab;
        public GameObject GreenWormPrefab;
        public GameObject BlueWormPrefab;
        public GameObject PurpleWormPrefab;

        public Transform SpawnPoint;
        public Button waveButton;
        public Text waveText;
        public GameObject WarningText;
        public GameObject TowersPanel;
        public AudioSource EnemyDeathSource;

        public event EventHandler<EventArgs> OnEnd;

        private const int EnemyGreenVirus = 1;
        private const int EnemyBlueVirus = 2;
        private const int EnemyPurpleVirus = 3;
        private const int EnemySmallpox = 4;
        private const int EnemyInfestedAntibody = 5;
        private const int EnemyInfestedMacrophage = 6;
        private const int EnemyGreenWorm = 7;
        private const int EnemyBlueWorm = 8;
        private const int EnemyPurpleWorm = 9;

        private float timer;
        private readonly List<int[]> waves = new List<int[]>();

        private int wave = -1;
        private int enemiesLeft;
        private int spawnedEnemy;
        private float delay = 1f;

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

            // WAVE 7
            waves.Add(new[]
            {
                EnemySmallpox, EnemySmallpox, EnemyPurpleVirus, EnemyPurpleVirus, EnemyBlueVirus, EnemySmallpox,
                EnemySmallpox, EnemyPurpleVirus, EnemySmallpox, EnemyPurpleVirus, EnemySmallpox, EnemyBlueVirus,
                EnemyGreenVirus, EnemyPurpleVirus, EnemySmallpox, EnemySmallpox, EnemySmallpox, EnemyPurpleVirus,
                EnemyBlueVirus, EnemyPurpleVirus, EnemySmallpox, EnemyGreenVirus
            });

            // WAVE 8
            waves.Add(new[]
            {
                EnemyGreenWorm, EnemyGreenWorm, EnemySmallpox, EnemyGreenWorm, EnemyGreenWorm, EnemyGreenWorm,
                EnemyGreenWorm, EnemyGreenWorm, EnemyGreenWorm, EnemySmallpox, EnemyGreenWorm, EnemyGreenWorm,
                EnemySmallpox, EnemyGreenWorm, EnemyGreenWorm, EnemyGreenWorm, EnemyGreenWorm, EnemyGreenWorm,
                EnemyGreenWorm, EnemyGreenWorm, EnemyGreenWorm, EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus
            });

            // WAVE 9
            waves.Add(new[]
            {
                EnemyGreenWorm, EnemyGreenWorm, EnemySmallpox, EnemyBlueWorm, EnemyGreenWorm, EnemyGreenWorm,
                EnemyBlueWorm, EnemyGreenWorm, EnemyGreenWorm, EnemySmallpox, EnemyBlueWorm, EnemyGreenWorm,
                EnemySmallpox, EnemyGreenWorm, EnemyBlueWorm, EnemyGreenWorm, EnemyGreenWorm, EnemyBlueWorm,
                EnemyGreenWorm, EnemyGreenWorm, EnemyBlueWorm, EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleWorm,
                EnemySmallpox, EnemySmallpox, EnemySmallpox
            });

            // WAVE 10
            waves.Add(new[]
            {
                EnemySmallpox, EnemySmallpox, EnemySmallpox, EnemySmallpox, EnemyBlueWorm, EnemyPurpleVirus,
                EnemyBlueWorm, EnemyBlueWorm, EnemyBlueWorm, EnemyBlueWorm, EnemyPurpleWorm, EnemyBlueWorm,
                EnemyBlueWorm, EnemySmallpox, EnemySmallpox, EnemySmallpox, EnemyPurpleVirus, EnemyBlueWorm,
                EnemyBlueWorm, EnemyBlueWorm, EnemyBlueWorm, EnemyBlueWorm, EnemyPurpleWorm
            });

            // WAVE 11
            waves.Add(new[]
            {
                EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus,
                EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus,
                EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus,
                EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus, EnemyGreenWorm, EnemyGreenVirus,
            });

            // WAVE 12
            waves.Add(new[]
            {
                EnemyPurpleWorm, EnemySmallpox, EnemySmallpox, EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm,
                EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm,
                EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm, EnemyPurpleWorm,
                EnemySmallpox, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus,
                EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyGreenVirus, EnemyPurpleVirus,
                EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus, EnemyPurpleVirus, EnemySmallpox,
                EnemyPurpleWorm
            });

            waveText.text = string.Format("Wave 0 / {0}", waves.Count);
        }

        private void Update()
        {
            if (enemiesLeft <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !Pause.Instance.Paused() && !Introduction.Instance.IsOpen())
                {
                    StartWave();
                }

                waveButton.interactable = true;
                return;
            }

            waveButton.interactable = false;

            timer -= Time.deltaTime;
            if (timer > 0) return;

            timer = delay;

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

                case EnemyInfestedMacrophage:
                    return InfestedMacrophagePrefab;

                case EnemyGreenWorm:
                    return GreenWormPrefab;

                case EnemyBlueWorm:
                    return BlueWormPrefab;

                case EnemyPurpleWorm:
                    return PurpleWormPrefab;

                default:
                    return null;
            }
        }

        public void KilledEnemy()
        {
            enemiesLeft--;
            EnemyDeathSource.Play();

            if (enemiesLeft < 0) enemiesLeft = 0;
            if (enemiesLeft > 0) return;

            WarningText.SetActive(false);
            TowersPanel.SetActive(true);

            DetailPanel.Instance.SetSellButtonState(true);

            if (OnEnd != null)
            {
                OnEnd.Invoke(this, EventArgs.Empty);
            }

            if (wave + 1 == waves.Count)
            {
                SceneManager.LoadScene("Win");
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void StartWave()
        {
            if (enemiesLeft > 0) return;

            DetailPanel.Instance.SetSellButtonState(false);

            delay = Mathf.Max(0.25f, delay - 0.2f);
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

        public bool IsWaveOngoing()
        {
            return enemiesLeft > 0;
        }
    }
}