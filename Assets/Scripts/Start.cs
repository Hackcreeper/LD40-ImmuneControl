using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD40
{
    public class Start : MonoBehaviour
    {
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}