using UnityEngine;

namespace LD40
{
    public class TowerPlacement : MonoBehaviour
    {
        public GameObject TowerPrefab;

        public GameObject CirclePrefab;

        public LayerMask PlaceMask;

        public LayerMask TowerMask;

        public static TowerPlacement Instance
        {
            get { return pInstance; }
        }

        private static TowerPlacement pInstance;

        private Tower Tower;

        private void Awake()
        {
            pInstance = this;

            Tower = Instantiate(TowerPrefab).GetComponent<Tower>();
        }

        private void Update()
        {
            if (!Tower.Placing)
            {
                Tower = Instantiate(TowerPrefab).GetComponent<Tower>();
            }
        }
    }
}