using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Game.Core;

namespace Game.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]private TMP_Text gameOverScoreText;
        
        private void Start() {
            ShowScoreAfterGameOver();
        }
        private void ShowScoreAfterGameOver()
        {
            gameOverScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
        }    
    }
}
