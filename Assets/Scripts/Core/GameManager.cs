using Game.SoundManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public static class GameManagerObserver//It's not observer pattern but..lol
    {
        private static bool _allowPause = true;

        public static bool CheckGameManagerWholeStatus()
        {
            if(!_allowPause)
            {
                return true;
            }
            if (!GameManager.Instance) return false;

            if (GameManager.Instance != null &&
                 GameManager.Instance.GameplayStatus == GameplayStatus.OnGoing &&
                 GameManager.Instance.GamePauseStatus == GamePauseStatus.UnPaused)
            {
                return true;
            }

            return false;
        }

        public static bool CheckGameManagerGameStatus()
        {
            if(!_allowPause)
            {
                return true;
            }
            if (!GameManager.Instance) return false;

            if (GameManager.Instance != null &&
                 GameManager.Instance.GameplayStatus == GameplayStatus.OnGoing)
            {
                return true;
            }

            return false;
        }
    }

    public enum GamePauseStatus
    {
        Paused,
        UnPaused
    }

    public enum GameplayStatus
    {
        None,
        OnGoing,
        End
    }
    public class GameManager : GenericSingleton<GameManager>
    {

        public UnityEvent OnSceneStart;
        public UnityEvent OnGamePaused;
        public UnityEvent OnGameResumed;

        public UnityEvent OnGameplayStart;
        public UnityEvent OnGameEnd;
        public UnityEvent OnBackToMain;

        [SerializeField, Header("Blur Material")]
        Material blurMaterial;

        private GamePauseStatus _gamePauseStatus = GamePauseStatus.UnPaused;

        private GameplayStatus _gameplayStatus = GameplayStatus.None;

        public GamePauseStatus GamePauseStatus { get=> _gamePauseStatus; }
        public GameplayStatus GameplayStatus { get=> _gameplayStatus;}

        private void InitializeScoreToZero()
        {
            ScoreManager.Instance.OnScoreSet?.Invoke(0);
        }

        private void PlayGameplayMusic()
        {
            SoundManager.Instance.PlayMusic(SoundType.GameplayTheme);
        }

        private void PlayGameOverMusic()
        {
            SoundManager.Instance.PlayMusic(SoundType.GameOverTheme);
        }
        private void PauseGame()
        {
            Time.timeScale = 0.0f;
            _gamePauseStatus = GamePauseStatus.Paused;

            BlurScreen(1f);
        }

        private void UnpauseGame()
        {
            Time.timeScale = 1.0f;
            _gamePauseStatus = GamePauseStatus.UnPaused;

            BlurScreen(0f);
        }

        private void StartGame()
        {
            //Debug.Log("Starting the game....");
            _gameplayStatus = GameplayStatus.OnGoing;
        }

        private void EndGame()
        {
            _gameplayStatus = GameplayStatus.End;
            //BlurScreen(1f);
        }

        private void ExitGameplay()
        {
            _gameplayStatus = GameplayStatus.None;
            BlurScreen(0f);
        }

        public void BlurScreen(float amount)
        {
            blurMaterial.SetFloat("_BlurAmount", amount);
        }
        private void Start() 
        {
            OnGamePaused.AddListener(PauseGame);
            OnGameResumed.AddListener(UnpauseGame);    
            OnGameplayStart.AddListener(StartGame);
            OnGameEnd.AddListener(EndGame);
            OnBackToMain.AddListener(ExitGameplay);

            OnGameplayStart.AddListener(InitializeScoreToZero);
            OnGameplayStart.AddListener(PlayGameplayMusic);
            OnGameEnd.AddListener(PlayGameOverMusic);

            OnSceneStart?.Invoke();
            GameManager.Instance.OnGameEnd.AddListener(()=> ScoreManager.Instance.OnScoreSet?.Invoke(ScoreManager.Instance.CurrentScore));
        }

        private void Update() {
            if(_gameplayStatus == GameplayStatus.None && Input.GetKeyDown(KeyCode.Space))
            {
                //if (SoundManager.Instance) SoundManager.Instance.PlayMusic(SoundType.ArenaTheme);
                OnGameplayStart?.Invoke();
            }
            else if(_gameplayStatus == GameplayStatus.OnGoing)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    if(_gamePauseStatus == GamePauseStatus.Paused)
                    {
                        OnGameResumed?.Invoke();    
                    }
                    else
                    {
                        OnGamePaused?.Invoke();
                    }
                }
            }
            
            if(_gameplayStatus == GameplayStatus.End)
            {
                float amount = blurMaterial.GetFloat("_BlurAmount");

                amount = amount + 0.3f * Time.deltaTime >= 1f ? 1f : amount + 0.3f * Time.deltaTime;

                BlurScreen(amount);
            }
        }

        private void OnDestroy() {
            OnSceneStart.RemoveAllListeners();
            OnGamePaused.RemoveAllListeners();
            OnGameResumed.RemoveAllListeners();
            OnGameplayStart.RemoveAllListeners();
            OnGameEnd.RemoveAllListeners();
            OnBackToMain.RemoveAllListeners();

            BlurScreen(0f);
        }
        
    }

}
