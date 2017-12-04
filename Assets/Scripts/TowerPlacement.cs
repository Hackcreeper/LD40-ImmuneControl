using UnityEngine;

namespace LD40
{
    public class TowerPlacement : MonoBehaviour
    {
        public GameObject CirclePrefab;
        public AudioSource PlacingSource;

        public LayerMask PlaceMask;
        public LayerMask TowerMask;

        public static TowerPlacement Instance { get; private set; }

        private Tower placingTower;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!placingTower) return;

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                Destroy(placingTower.gameObject);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void SpawnTower(GameObject prefab)
        {
            if (placingTower) return;

            if (DetailPanel.Instance.IsOpen())
            {
                DetailPanel.Instance.Close();
            }

            placingTower = Instantiate(prefab).GetComponent<Tower>();
            placingTower.transform.position = new Vector3(0, -1000, 0);
        }

        public void Placed()
        {
            placingTower = null;
            PlacingSource.Play();
        }

        public void Cancel()
        {
            if (!placingTower) return;

            Destroy(placingTower.gameObject);
        }

        public bool IsPlacing()
        {
            return placingTower != null;
        }
    }
}