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

        [HideInInspector] public int Killed;

        [HideInInspector] public int Value;

        protected readonly List<Enemy> attachedEnemies = new List<Enemy>();

        private bool placing = true;
        private Color originalColor;
        private MeshRenderer circle;
        private MeshRenderer renderer;

        private void Start()
        {
            renderer = GetComponentInChildren<MeshRenderer>();
            originalColor = renderer.material.color;

            OnStart();
        }

        private void Update()
        {
            if (!placing)
            {
                OnUpdate();
                return;
            }

            CreateCircleIfNotExists();
            circle.transform.position = transform.position + new Vector3(0, .1f, 0);

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, 1000, TowerPlacement.Instance.PlaceMask))
            {
                // TODO: Some logic here to hide the tower completley
                return;
            }

            var canPlace = Physics.OverlapSphere(hit.point, .55f, TowerPlacement.Instance.TowerMask).Length == 0;
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

        public void InformEnemy(Enemy enemy)
        {
            attachedEnemies.Add(enemy);
        }

        public void InformDeath(Enemy enemy)
        {
            attachedEnemies.Remove(enemy);
        }

        private void OnMouseDown()
        {
            if (placing) return;

            DetailPanel.Instance.Open(this);
        }

        private void Place()
        {
            if (!Cells.Instance.Sub(Price)) return;

            placing = false;
            gameObject.layer = LayerMask.NameToLayer("Tower");
            SetColor(originalColor, 1f, false, true);
            Destroy(circle.gameObject);
            TowerPlacement.Instance.Placed();

            OnPlace();
        }

        private void CreateCircleIfNotExists()
        {
            if (circle != null) return;

            circle = Instantiate(TowerPlacement.Instance.CirclePrefab).GetComponent<MeshRenderer>();
            circle.transform.localScale = new Vector3(Radius*2, 0.01f, Radius*2);
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
    }
}