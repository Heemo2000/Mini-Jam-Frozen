using System;
using UnityEngine;
using Game.Core;
using Game.UI;
using UnityEngine.Events;

using Random = UnityEngine.Random;
using System.Collections.Generic;

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

            List<PowerUpData> copyedDatas = new List<PowerUpData>((PowerUpData[]) powerUpDatas.Clone());

            PowerUpData firstPowerupIndex = copyedDatas[Random.Range(0,copyedDatas.Count)];
            copyedDatas.Remove(firstPowerupIndex);

            //Random.InitState((int)System.DateTime.Now.Ticks);
            PowerUpData secondPowerupIndex = copyedDatas[Random.Range(0, copyedDatas.Count)];

            powerupUI1.SetUI(firstPowerupIndex);
            powerupUI2.SetUI(secondPowerupIndex);
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
