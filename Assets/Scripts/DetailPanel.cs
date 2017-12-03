using UnityEngine;
using UnityEngine.UI;

namespace LD40
{
    public class DetailPanel : MonoBehaviour
    {
        public static DetailPanel Instance { get; private set; }

        public Transform Panel;
        public Text Title;
        public Image Image;
        public Text Killed;
        public Text Sell;
        public Button SellButton;
        
        private Tower currentTower;

        private void Start()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!currentTower || !Input.GetKeyDown(KeyCode.Escape)) return;

            Close();
        }

        public void Open(Tower tower)
        {
            currentTower = tower;

            Panel.gameObject.SetActive(true);

            Title.text = tower.Name;
            Image.sprite = tower.Image;
            Killed.text = string.Format("Killed {0} enemies!", tower.Killed);
            Sell.text = string.Format("Sell for {0}", tower.Value);
        }

        public bool IsOpen()
        {
            return currentTower != null;
        }

        public void Close()
        {
            currentTower = null;
            Panel.gameObject.SetActive(false);
        }

        public void SellTower()
        {
            if (EnemySpawner.Instance.IsWaveOngoing())
            {
                return;
            }
            
            Cells.Instance.Add(currentTower.Value);
            Destroy(currentTower.gameObject);
            Close();
        }

        public void SetSellButtonState(bool activated)
        {
            SellButton.interactable = activated;
        }
    }
}