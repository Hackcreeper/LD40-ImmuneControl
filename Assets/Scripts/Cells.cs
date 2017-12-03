using UnityEngine;
using UnityEngine.UI;

namespace LD40
{
    public class Cells : MonoBehaviour
    {
        public static Cells Instance { get; private set; }

        public int Amount = 1000;

        public Text Text;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            Text.text = string.Format("Cells: {0}", Amount);
        }

        public void Add(int value)
        {
            Amount += value;
        }

        public bool Sub(int wanted)
        {
            if (Amount < wanted) return false;

            Amount -= wanted;
            return true;
        }

        public bool Check(int wanted)
        {
            return Amount >= wanted;
        }
    }
}