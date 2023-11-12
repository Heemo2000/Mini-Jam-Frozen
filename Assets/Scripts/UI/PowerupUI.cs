using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.PowerupStuff;
namespace Game
{
    public class PowerupUI : MonoBehaviour
    {
        [SerializeField]private TMP_Text powerupName;
        [SerializeField]private Image goodAbilityImage;
        [SerializeField]private Image badAbilityImage;

        public void SetUI(PowerUpData data)
        {
            powerupName.text = data.InfoText;
            goodAbilityImage.sprite = data.GoodInfoImage;
            badAbilityImage.sprite = data.BadInfoImage;
        }
        
    }
}
