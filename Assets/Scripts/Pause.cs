using UnityEngine;

namespace LD40
{
    public class Pause : MonoBehaviour
    {
        public static Pause Instance { get; private set; }
        
        public Transform PauseScreen;

        private float originalTime;
        private bool isOpen;
        private bool faked = false;

        private void Awake()
        {
            Instance = this;
        }
        
        private void Update()
        {
            if (faked) return;
            if (DetailPanel.Instance.IsOpen()) return;
            if (TowerPlacement.Instance.IsPlacing()) return;
            if (Introduction.Instance.IsOpen()) return;

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

        public void FakePause(bool enable)
        {
            if (!faked && !enable) return;
            
            if (enable)
            {
                originalTime = Time.timeScale;
                Time.timeScale = 0;
                isOpen = true;
                faked = true;
            }
            else
            {
                Time.timeScale = originalTime;
                isOpen = false;
                faked = false;
            }
        }
    }
}