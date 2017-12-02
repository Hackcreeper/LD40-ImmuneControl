using UnityEngine;
using UnityEngine.Rendering;

namespace LD40
{
    public class Tower : MonoBehaviour
    {
        public string Name;
        public Sprite Image;
        public int Price;
        
        [HideInInspector]
        public int Killed;
        
        [HideInInspector]
        public int Value;
        
        private bool placing = true;
        private Color originalColor;
        private MeshRenderer circle;

        private void Start()
        {
            originalColor = GetComponent<MeshRenderer>().material.color;
        }
        
        private void Update()
        {
            if (!placing) return;

            CreateCircleIfNotExists();
            
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
        }
        
        private void CreateCircleIfNotExists()
        {
            if (circle != null) return;
            
            circle = Instantiate(TowerPlacement.Instance.CirclePrefab).GetComponent<MeshRenderer>();
            circle.transform.SetParent(transform);
            circle.transform.localPosition = new Vector3(0, .1f, 0);
        }

        private void SetColor(Color color, float alpha, bool colorizeCircle, bool shadows)
        {
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();

            if (shadows)
            {
                meshRenderer.receiveShadows = true;
                meshRenderer.shadowCastingMode = ShadowCastingMode.On;
            }
            else
            {
                meshRenderer.receiveShadows = false;
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;   
            }
            
            meshRenderer.material.color = new Color(
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