using UnityEngine;

namespace LD40
{
    public class RotationAnimation : MonoBehaviour
    {
        private float speed;

        private void Start()
        {
            speed = Random.Range(1f, 10f);
        }

        private void Update()
        {
            transform.Rotate(new Vector3(0, speed, 0));
        }
    }
}