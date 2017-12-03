using UnityEngine;

namespace LD40
{
    public class RotationAnimation : MonoBehaviour
    {
        public float Min = 1f;
        public float Max = 10f;
        
        private float speed;

        private void Start()
        {
            speed = Random.Range(Min, Max);
        }

        private void Update()
        {
            transform.Rotate(new Vector3(0, speed, 0));
        }
    }
}