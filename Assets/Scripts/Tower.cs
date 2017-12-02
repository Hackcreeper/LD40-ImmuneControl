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

            SetOriginal();
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
                setGreen();
            }
            else
            {
                SetRed();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Place();
            }
        }
        
        private void SetOriginal()
        {
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();
            
            meshRenderer.receiveShadows = true;
            meshRenderer.shadowCastingMode = ShadowCastingMode.On;
            
            meshRenderer.material.color = originalColor;
        }
        
        private void SetRed()
        {
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();

            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            
            meshRenderer.material.color = new Color(
                1f, 0f, 0f, 0.5f
            );
            
            Circle.material.color = new Color(
                1f, 0f, 0f, 0.15f
            );
        }
        
        private void setGreen()
        {
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();
            
            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            
            meshRenderer.material.color = new Color(
                0f, 1f, 0f, 0.5f
            );
            
            Circle.material.color = new Color(
                0f, 1f, 0f, 0.15f
            );
        }

    }
}