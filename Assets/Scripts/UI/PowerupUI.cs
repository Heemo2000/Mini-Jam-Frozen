using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.PowerupStuff;
using Game.Player;
namespace Game.UI
{
    public class PowerupUI : MonoBehaviour
    {
        [SerializeField]private PowerUpData powerUpData;
        [SerializeField]private TMP_Text powerupName;
        [SerializeField]private Image goodAbilityImage;
        [SerializeField]private Image badAbilityImage;

        [SerializeField]private PlayerController player;

        private Button _button;


        public void SetPlayerPowerup()
        {
            powerUpData.UsePowerUps(player);
            PowerupManager.Instance.SetPowerupCanvasVisible(false);
        }
        public void SetUI(PowerUpData data)
        {
            powerupName.text = data.InfoText;
            goodAbilityImage.color = Color.white;
            badAbilityImage.color = Color.white;
            
            goodAbilityImage.sprite = data.GoodInfoImage;
            badAbilityImage.sprite = data.BadInfoImage;
            powerUpData = data;
        }

        private void Awake() {
            _button = GetComponent<Button>();
        }

        private void Start() 
        {
            _button.onClick.AddListener(SetPlayerPowerup);
        }

        private void OnDestroy() 
        {
            _button.onClick.RemoveAllListeners();    
        }
        
    }
}
