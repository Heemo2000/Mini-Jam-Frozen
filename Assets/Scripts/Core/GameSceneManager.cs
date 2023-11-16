using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class GameSceneManager : MonoBehaviour
    {
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {

            //Application.OpenURL("https://itch.io/jam/mini-jam-145-frozen/rate/2365087");
            //#elif UNITY_WEBGL
            //Application.OpenURL("https://itch.io/jam/mini-jam-145-frozen/rate/2365087");
//#else

#if UNITY_EDITOR || !UNITY_WEBGL
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
