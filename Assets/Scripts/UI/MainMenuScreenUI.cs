using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Core;

namespace Game.UI
{
    public class MainMenuScreenUI : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(StringHolder.GameplaySceneName);   
        }
    }

}
