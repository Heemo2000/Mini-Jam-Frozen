using Game.StateMachineManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerDefaultState : MonoBehaviour , IState
    {
        private PlayerController _controller;

        private Rigidbody2D _rb;

        private PlayerShooter _shooter;//Shoot a snowball with this class

        private bool _isReloading = false;

        void Awake()
        {
            _controller = GetComponent<PlayerController>();

            _rb = GetComponent<Rigidbody2D>();

            SetShooter();
        }

        public void OnEnter()
        {
            StartCoroutine(DelayShoot());

            Debug.Log("Start Player Default State");
        }

        public void OnExit()
        {
            StopCoroutine(DelayShoot());
        }

        public void OnUpdate()
        {
            SnowGaugeUpdate();

            Move();

            ShootSnowBall();
        }

        #region Movement

        Vector2 dir = new Vector3();
        private void Move()// Moveing functions
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            dir.Set(h, v);
            dir.Normalize();

            _rb.MovePosition(_rb.position + dir * _controller.FinalSpeed * Time.deltaTime);
        }

        #endregion Movement

        #region Shooting

        public void ShootSnowBall()
        {
            if (_isReloading) return;
            if (!Input.GetMouseButtonDown(0)) return;//Shoot only when click the mouse left button

            _shooter.Attack();//Well It's actually Shoot() but... lol
            _controller.StartCoroutine(DelayShoot());

            _controller.ChangeSnowGauge(_controller.ShootingSnowGauge);
        }

        private void SetShooter()//Find Shooter at first Time
        {
            _shooter = _controller.GetComponentInChildren<PlayerShooter>();
        }

        IEnumerator DelayShoot()
        {
            _isReloading = true;

            yield return new WaitForSeconds(_controller.ShootingDelay);

            _isReloading = false;
        }

        #endregion Shooting

        private void SnowGaugeUpdate()//Fuction that will run inside of Update
        {
            _controller.ChangeSnowGauge(-_controller.SnowGaugeDecreaseSpeed * Time.deltaTime);//Decreasing Snow Gauge

        }
    }
}
