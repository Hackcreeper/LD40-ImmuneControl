using UnityEngine;

namespace LD40
{
    public class Pause : MonoBehaviour
    {
        public static Pause Instance { get; private set; }
        
        public Transform PauseScreen;

        private float originalTime;
        private bool isOpen;

        private void Awake()
        {
            Instance = this;
        }
        
        private void Update()
        {
            if (DetailPanel.Instance.IsOpen()) return;
            if (TowerPlacement.Instance.IsPlacing()) return;

            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            if (!isOpen)
            {
                PauseScreen.gameObject.SetActive(true);
                originalTime = Time.timeScale;
                Time.timeScale = 0;
                isOpen = true;                    
            }
            else
            {
                PauseScreen.gameObject.SetActive(false);
                Time.timeScale = originalTime;
                isOpen = false;
            }
        }

        public bool Paused()
        {
            return isOpen;
        }
    }
}