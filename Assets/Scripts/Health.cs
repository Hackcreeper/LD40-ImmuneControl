using UnityEngine;
using UnityEngine.UI;

namespace LD40
{
    public class Health : MonoBehaviour
    {
        public static Health Instance { get; private set; }
        
        public int Amount;
        public int Max;
        
        public RectTransform HealthFill;
        public Text HealthText;

        private float width;

        private void Start()
        {
            Instance = this;
            width = HealthFill.sizeDelta.x;
        }
        
        private void Update()
        {
            HealthFill.sizeDelta = new Vector2(
                width / Max * Amount,
                HealthFill.sizeDelta.y
            );

            HealthText.text = string.Format("{0} / {1}", Amount, Max);
        }

        public void Sub(int value)
        {
            Amount -= value;
        }
    }
}