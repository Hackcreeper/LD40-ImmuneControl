using UnityEngine;
using UnityEngine.UI;

namespace LD40
{
    public class Notification : MonoBehaviour
    {
        public static Notification Instance { get; private set; }

        public Text NotificationText;

        private float timer;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowRandomDisease()
        {
            NotificationText.color = Color.white;
            NotificationText.text = string.Format("You got infected with {0}", Diseases.Random());
            timer = 2f;
        }

        public void ShowWarning(string text)
        {
            NotificationText.color = Color.yellow;
            NotificationText.text = text;
            timer = 2f;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0) return;

            NotificationText.text = "";
            timer = 0;
        }
    }
}