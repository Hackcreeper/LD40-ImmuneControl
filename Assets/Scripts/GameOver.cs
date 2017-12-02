using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD40
{
    public class GameOver : MonoBehaviour
    {
        public void Replay()
        {
            SceneManager.LoadScene("Game");
        }
    }
}