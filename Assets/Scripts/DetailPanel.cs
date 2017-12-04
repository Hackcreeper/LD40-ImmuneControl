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
        public Text UpgradeName;
        public Text UpgradeDescription;
        public Text Upgrade;
        public Button UpgradeButton;
        
        private Tower currentTower;

        private void Start()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!currentTower) return;

            Killed.text = string.Format("Killed {0} enemies!", currentTower.Killed);
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }

        public void Open(Tower tower)
        {
            if (currentTower != null)
            {
                Close();
            }
            
            currentTower = tower;

            Panel.gameObject.SetActive(true);

            Title.text = tower.Name;
            Image.sprite = tower.Image;
            Killed.text = string.Format("Killed {0} enemies!", tower.Killed);
            Sell.text = string.Format("Sell for {0}", tower.Value);

            if (!tower.IsUpgraded())
            {
                UpgradeButton.gameObject.SetActive(true);
                UpgradeName.text = tower.UpgradeName;
                UpgradeDescription.text = tower.UpgradeDescription;
                Upgrade.text = string.Format("Upgrade for {0}", tower.UpgradePrice);                
            }
            else
            {
                UpgradeButton.gameObject.SetActive(false);
                UpgradeName.text = "Fully upgraded";
                UpgradeDescription.text = "";                
            }
        }

        public bool IsOpen()
        {
            return currentTower != null;
        }

        public void Close()
        {
            if (currentTower == null) return;
            
            currentTower.OnDetailClose();
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
            UpgradeButton.interactable = activated;
        }

        public bool IsMe(Tower other)
        {
            return currentTower == other;
        }

        public void UpgradeTower()
        {
            if (EnemySpawner.Instance.IsWaveOngoing()) return;
            if (currentTower.IsUpgraded()) return;

            if (!Cells.Instance.Check(currentTower.UpgradePrice)) return;
            
            Cells.Instance.Sub(currentTower.UpgradePrice);
            currentTower.Upgrade();
                
            Title.text = currentTower.Name;
            Image.sprite = currentTower.Image;
            Sell.text = string.Format("Sell for {0}", currentTower.Value);
            
            UpgradeButton.gameObject.SetActive(false);
            UpgradeName.text = "Fully upgraded";
            UpgradeDescription.text = "";
        }
    }
}