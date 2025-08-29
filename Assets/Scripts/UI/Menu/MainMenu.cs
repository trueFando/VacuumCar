using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Level1");
        }
    }
}