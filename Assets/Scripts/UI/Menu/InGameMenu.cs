using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class InGameMenu : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoHome()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}