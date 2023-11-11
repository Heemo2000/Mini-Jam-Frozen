using Game.StateMachineManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerFrozenState : MonoBehaviour , IState
    {
        private PlayerController _controller;

        private float _time;

        void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        public void OnEnter()
        {
            _time = 0;

            Debug.Log("Start Player Frozen State");
        }

        public void OnExit()
        {
            _controller.SetSnowGauge(0);// Set Snow Gauge to 0 when end to froze
        }

        public void OnUpdate()
        {
            _time += Time.deltaTime;

            if(_time >= _controller.FreezeTime)
            {
                _controller.IsFrozen = false;//Make isFrosen to false then transition will activate
            }
        }

        public void OnFixedUpdate()
        {

        }
    }

}
