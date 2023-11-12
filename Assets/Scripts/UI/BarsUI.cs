using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BarsUI : MonoBehaviour
    {
        [SerializeField]private Image fillImage;
        public void SetFillAmount(float currentValue,float maxValue)
        {
            fillImage.fillAmount = currentValue/maxValue;
        }
    }

}
