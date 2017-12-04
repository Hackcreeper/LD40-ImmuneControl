using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LD40
{
    public class Tower : MonoBehaviour
    {
        public string Name;
        public Sprite Image;
        public int Price;
        public float Radius = 2f;
        public int Damage = 5;
        public bool Sticky;

        public string UpgradeName;
        public string UpgradeDescription;
        public int UpgradePrice;
        
        public List<Enemy> AttachedEnemies = new List<Enemy>();

        [HideInInspector] public int Killed;

        [HideInInspector] public int Value;

        protected MeshRenderer circle;
        protected bool upgraded;
        
        private bool placing = true;
        private Color originalColor;
        private Renderer renderer;

        private void Start()
        {
            renderer = GetComponentInChildren<MeshRenderer>() ??
                       (Renderer) GetComponentInChildren<SkinnedMeshRenderer>();


            originalColor = renderer.material.color;

            Value = Mathf.FloorToInt(Price * 0.6f);

            OnStart();
        }

        private void Update()
        {
            if (circle != null)
            {
                circle.transform.position = transform.position + new Vector3(0, .1f, 0);
            }
            
            if (!placing)
            {
                OnUpdate();
                return;
            }

            CreateCircleIfNotExists();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            var canPlace = Physics.Raycast(ray, out hit, 1000, TowerPlacement.Instance.PlaceMask);

            canPlace = canPlace && Physics.OverlapSphere(hit.point, .55f, TowerPlacement.Instance.TowerMask).Length ==
                       0;

            if (!hit.collider.CompareTag("Level"))
            {
                canPlace = false;
            }

            transform.position = hit.point;

            canPlace = canPlace && Cells.Instance.Check(Price);

            SetColor(canPlace ? Color.green : Color.red, 0.5f, true, false);

            if (canPlace && Input.GetMouseButtonDown(0))
            {
                Place();
            }
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnPlace()
        {
        }

        protected virtual void OnUpgrade()
        {
        }

        public void InformEnemy(Enemy enemy)
        {
            AttachedEnemies.Add(enemy);
        }

        public void InformDeath(Enemy enemy)
        {
            AttachedEnemies.Remove(enemy);
        }

        private void OnMouseDown()
        {
            if (placing) return;

            CreateCircleIfNotExists();
            SetCircleColor(new Color(0, 1, 0, 0.15f));

            DetailPanel.Instance.Open(this);
        }

        public void OnDetailClose()
        {
            if (circle != null)
            {
                Destroy(circle.gameObject);   
            }
        }

        private void Place()
        {
            if (!Cells.Instance.Sub(Price)) return;

            placing = false;
            gameObject.layer = LayerMask.NameToLayer("Tower");
            SetColor(originalColor, originalColor.a, false, true);
            Destroy(circle.gameObject);
            TowerPlacement.Instance.Placed();

            OnPlace();
        }

        protected void CreateCircleIfNotExists()
        {
            if (circle != null) return;

            circle = Instantiate(TowerPlacement.Instance.CirclePrefab).GetComponent<MeshRenderer>();
            circle.transform.localScale = new Vector3(Radius * 2, 0.01f, Radius * 2);
            circle.transform.position = transform.position + new Vector3(0, .1f, 0);
        }

        private void SetColor(Color color, float alpha, bool colorizeCircle, bool shadows)
        {
            if (shadows)
            {
                renderer.receiveShadows = true;
                renderer.shadowCastingMode = ShadowCastingMode.On;
            }
            else
            {
                renderer.receiveShadows = false;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
            }

            renderer.material.color = new Color(
                color.r, color.g, color.b, alpha
            );

            if (colorizeCircle)
            {
                circle.material.color = new Color(
                    color.r, color.g, color.b, 0.15f
                );
            }
        }

        private void SetCircleColor(Color color)
        {
            circle.material.color = color;
        }

        private void OnDestroy()
        {
            if (circle)
            {
                Destroy(circle.gameObject);
            }
        }

        public bool HasAttached()
        {
            return AttachedEnemies.Count > 0;
        }

        public bool IsUpgraded()
        {
            return upgraded;
        }

        public void Upgrade()
        {
            Name = UpgradeName;
            Value = Mathf.FloorToInt((Price + UpgradePrice) * 0.6f);
            upgraded = true;

            OnUpgrade();
        }
    }
}