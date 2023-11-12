using System;
using UnityEngine;
using Game.Core;
using Game.UI;
using UnityEngine.Events;

using Random = UnityEngine.Random;
namespace Game.PowerupStuff
{
    public class PowerupManager : GenericSingleton<PowerupManager>
    {
        [SerializeField]private PowerUpData[] powerUpDatas;

        [SerializeField]private Canvas powerupCanvas;
        [SerializeField]private PowerupUI powerupUI1;
        [SerializeField]private PowerupUI powerupUI2;
        
        public UnityEvent OnGettingPowerups;
        public void SetPowerupCanvasVisible(bool visible)
        {
            powerupCanvas.gameObject.SetActive(visible);
        }
        private void GetRandomPowerups()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            int firstPowerupIndex = Random.Range(0, powerUpDatas.Length);
            
            Random.InitState((int)System.DateTime.Now.Ticks);
            int secondPowerupIndex = Random.Range(0, powerUpDatas.Length);

            powerupUI1.SetUI(powerUpDatas[firstPowerupIndex]);
            powerupUI2.SetUI(powerUpDatas[secondPowerupIndex]);
        }

        private void Start() 
        {
            OnGettingPowerups.AddListener(GetRandomPowerups);
        }

        private void OnDestroy() 
        {
            OnGettingPowerups.RemoveAllListeners();
        }
    }
}
