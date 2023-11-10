using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Core;

namespace Game.UI
{
    public class PauseScreenUI : MonoBehaviour
    {
        
        public void BackToMain()
        {
            SceneManager.LoadScene(StringHolder.MainMenuSceneName);
        }
        public void Exit()
        {
            Application.Quit();
        }
    }

}
