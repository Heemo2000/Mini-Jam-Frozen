using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using Game.PowerupStuff;

namespace Game.Enemy
{
    public class EnemyDeathCounter : GenericSingleton<EnemyDeathCounter>
    {
        [SerializeField]private int checkKillCount = 4;

        public UnityEvent<string> OnKillCountNotified;
        private int _currentCount = 0;

        public void IncreaseCount()
        {
            _currentCount++;
            if(_currentCount % checkKillCount == 0)
            {
                GameManager.Instance.OnGamePaused?.Invoke();
                PowerupManager.Instance.OnGettingPowerups?.Invoke();
            }
            OnKillCountNotified?.Invoke(_currentCount.ToString());
        }
    }
}
