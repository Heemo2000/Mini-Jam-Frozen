using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using UnityEngine.UIElements;
//using UnityEngine.UI;

namespace Game.PowerupStuff
{
    public class PowerUpInfo//Use this class for powerups
    {
        public bool UpgradeShooter = false;

        public float SnowBallAttackAmount = 0f;
        public float SnowBallSizeAmount = 0f;
        public float PlayerSpeedAmount = 0f;

        public float SnowGaugeIncreaseAmount = 0f;
        public float SnowGaugeDecreaseAmount = 0f;
        public float SnowGaugeSpeedDecreaseAmount = 0f;

    }


    //You can use this Data for PowerUp players
    //The all infomation of PowerUp will be in here
    [CreateAssetMenu(fileName = "PowerUp Data", menuName = "PowerUp/PowerUp Data")]
    public class PowerUpData : ScriptableObject
    {

        [SerializeField, Space(10f)]
        string _infoText;//You can use this when making UI

        [SerializeField, Space(10f)]
        Sprite _badInfoImage;//You can choose text or Image for UI

        [SerializeField]
        Sprite _goodInfoImage;//You can choose text or Image for UI

        public string InfoText {
            get
            {
                string text = _infoText.Replace("\\n", "\n");
                return text;
            }
        }
        public Sprite BadInfoImage { get => _badInfoImage; }
        public Sprite GoodInfoImage { get => _goodInfoImage; }


        [Header("Good Abilities")]
        [SerializeField] bool _upgradeShooter = false;
        [SerializeField] float _snowBallAttackAmount;
        [SerializeField] float _snowBallSizeAmount;
        [SerializeField] float _playerSpeedAmount;

        [Header("Bad Abilities")]
        [SerializeField] float _snowGaugeIncreaseAmount;
        [SerializeField] float _snowGaugeDecreaseAmount;
        [SerializeField] float _snowGaugeSpeedDecreaseAmount;

        public PowerUpInfo UsePowerUps()//You can use this mathod and make powerUp Info and use if for PowerUp() in PlayerController
        {
            PowerUpInfo info = new PowerUpInfo();
            CopyPowerUpInfo(info);

            return info;
        }

        public void UsePowerUps(PlayerController playerController)//it PowerUps directly to Player
        {
            PowerUpInfo info= UsePowerUps();

            playerController.PowerUp(info);

            return;
        }

        void CopyPowerUpInfo(PowerUpInfo info)
        {
            info.UpgradeShooter = _upgradeShooter;

            info.SnowBallAttackAmount = _snowBallAttackAmount;
            info.SnowBallSizeAmount = _snowBallSizeAmount;
            info.PlayerSpeedAmount = _playerSpeedAmount;

            info.SnowGaugeIncreaseAmount = _snowGaugeIncreaseAmount;
            info.SnowGaugeDecreaseAmount = _snowGaugeDecreaseAmount;
            info.SnowGaugeSpeedDecreaseAmount = _snowGaugeSpeedDecreaseAmount;
        }

    }
}
