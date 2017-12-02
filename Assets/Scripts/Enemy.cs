using UnityEngine;

namespace LD40
{
    public class Enemy : MonoBehaviour
    {
        public float Speed = 5f;

        private int currentNode;

        private float progress;

        private Vector3? targetPosition;

        private void Start()
        {
            targetPosition = Route.Instance.GetPosition(currentNode);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, Speed * Time.deltaTime);

            if (!(Vector3.Distance(transform.position, targetPosition.Value) <= 0.1f)) return;
            
            progress = 0;
                
            currentNode++;
            targetPosition = Route.Instance.GetPosition(currentNode);

            if (targetPosition == null)
            {
                Destroy(gameObject);
            }
        }
    }
}