using UnityEngine;

namespace LD40
{
    public class TowerPlacement : MonoBehaviour
    {
        public GameObject CirclePrefab;

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
            if (placingTower)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    Destroy(placingTower.gameObject);
                }
            }
        }

        public void SpawnTower(GameObject prefab)
        {
            if (placingTower)
            {
                return;
            }
            
            placingTower = Instantiate(prefab).GetComponent<Tower>();
        }

        public void Placed()
        {
            placingTower = null;
        }
    }
}