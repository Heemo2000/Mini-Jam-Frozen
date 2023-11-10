using System;
using UnityEngine;
using TMPro;
using Game.Core;

namespace Game.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField]private TMP_Text scoreText;
        
        private void Start() 
        {
            ScoreManager.Instance.OnScoreSet += SetScoreText;
        }
        public void SetScoreText(float score)
        {
            scoreText.text = ((int)score).ToString();
        }
    
        private void OnDestroy() 
        {
            ScoreManager.Instance.OnScoreSet -= SetScoreText;    
        }
    }

}
