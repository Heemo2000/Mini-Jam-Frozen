using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] Slider slider;

        Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            slider.value = Mathf.Lerp(slider.value, health.CurrentAmount / health.MaxHealth, 10 * Time.deltaTime);
        }
    }
}
