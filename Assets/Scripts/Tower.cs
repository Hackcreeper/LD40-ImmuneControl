using UnityEngine;
using UnityEngine.Rendering;

namespace LD40
{
    public class Tower : MonoBehaviour
    {
        public bool Placing = true;

        private bool CanPlace;

        private Color originalColor;

        private MeshRenderer Circle;

        private void Start()
        {
            originalColor = GetComponent<MeshRenderer>().material.color;
        }
        
        private void Place()
        {
            if (!CanPlace) return;
            
            Placing = false;
            gameObject.layer = LayerMask.NameToLayer("Tower");

            SetColor(originalColor, 1f, false, true);
            Destroy(Circle.gameObject);
        }

        private void Update()
        {
            if (!Placing) return;

            if (Circle == null)
            {
                Circle = Instantiate(TowerPlacement.Instance.CirclePrefab).GetComponent<MeshRenderer>();
                Circle.transform.SetParent(transform);
                Circle.transform.localPosition = new Vector3(0, .1f, 0);
            }
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
                
            if (!Physics.Raycast(ray, out hit, 1000, TowerPlacement.Instance.PlaceMask))
            {
                // TODO: Some logic here to hide the tower completley
                return;
            }

            CanPlace = Physics.OverlapSphere(hit.point, .55f, TowerPlacement.Instance.TowerMask).Length == 0;
            transform.position = hit.point;

            if (CanPlace)
            {
                SetColor(Color.green, 0.5f, true, false);
            }
            else
            {
                SetColor(Color.red, 0.5f, true, false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Place();
            }
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
                Circle.material.color = new Color(
                    color.r, color.g, color.b, 0.15f
                );                
            }
        }
    }
}