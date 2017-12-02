using UnityEngine;

namespace LD40
{
    public class Route : MonoBehaviour
    {
        public Transform[] Nodes;
        
        public static Route Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public Vector3? GetPosition(int idx)
        {
            if (idx >= Nodes.Length)
            {
                return null;
            }
            
            return Nodes[idx].position;
        }
    }
}