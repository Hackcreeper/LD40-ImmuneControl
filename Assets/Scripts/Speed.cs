using UnityEngine;
using UnityEngine.UI;

namespace LD40
{
    public class Speed : MonoBehaviour
    {
        public Text ButtonText;
        
        private bool isFast = false;

        public void Toggle()
        {
            if (!isFast)
            {
                isFast = true;
                Time.timeScale = 2;

                ButtonText.text = "«« Normal speed";
            }
            else
            {
                isFast = false;
                Time.timeScale = 1;

                ButtonText.text = "Fast forward »»";
            }
        }

        private void Update()
        {
            if (Pause.Instance.Paused()) return;

            if (Input.GetKeyDown(KeyCode.F))
            {
                Toggle();
            }
        }
    }
}