using UnityEngine;

namespace LD40
{
    public class Introduction : MonoBehaviour
    {
        public static Introduction Instance { get; private set; }
        
        public Transform[] Tabs;
        public Transform introObject;

        private int current;
        private bool isOpen = true;

        private void Awake()
        {
            Instance = this;
        }
        
        public void Open(int tab)
        {
            Tabs[current].gameObject.SetActive(false);
            Tabs[tab].gameObject.SetActive(true);

            current = tab;
        }

        public void Close()
        {
            Pause.Instance.FakePause(false);
            introObject.gameObject.SetActive(false);

            Tabs[current].gameObject.SetActive(false);
            current = 0;
            Tabs[current].gameObject.SetActive(true);

            isOpen = false;
        }

        public void Reopen()
        {
            TowerPlacement.Instance.Cancel();
            Pause.Instance.FakePause(true);
            
            isOpen = true;
            introObject.gameObject.SetActive(true);
        }

        public bool IsOpen()
        {
            return isOpen;
        }
    }
}