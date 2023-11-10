using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game.Core;
using Game.SoundManagement;

namespace Game.UI
{
    public class ButtonFunction : MonoBehaviour
    {
        private Button _button;
    
        private void Awake() {
            _button = GetComponent<Button>();
        }
        private void Start() 
        {
            if(SoundManager.Instance != null)
            {
                _button.onClick.AddListener(()=>SoundManager.Instance.PlaySFX(SoundType.ButtonClick));
            }
        }
        public void BackToMain()
        {
            SceneManager.LoadScene(StringHolder.MainMenuSceneName);
            GameManager.Instance.OnBackToMain?.Invoke();
        }
    
        public void Exit()
        {
            Application.Quit();
        }
        private void OnDestroy() {
            _button.onClick.RemoveAllListeners();
        }
    }

}
