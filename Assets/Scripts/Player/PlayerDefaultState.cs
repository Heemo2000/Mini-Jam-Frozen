using Game.SoundManagement;
using Game.StateMachineManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerDefaultState : MonoBehaviour , IState
    {
        [SerializeField] Transform _shooterPos;

        private PlayerController _controller;

        private Rigidbody2D _rb;

        private Animator _animator;

        private PlayerShooter _shooter;//Shoot a snowball with this class

        private bool _isReloading = false;

        

        void Awake()
        {
            _controller = GetComponent<PlayerController>();

            _rb = GetComponent<Rigidbody2D>();

            _animator = GetComponent<Animator>();

            _isReloading = false;

            SetShooter();
        }

        public void OnEnter()
        {
            StartCoroutine("DelayShoot");

            Debug.Log("Start Player Default State");
        }

        public void OnExit()
        {
            StopCoroutine("DelayShoot");
        }

        public void OnUpdate()
        {
            SnowGaugeUpdate();

            ShootSnowBall();
        }

        public void OnFixedUpdate()
        {
            Move();

            UpdateAnimator();
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

            //if (SoundManager.Instance) SoundManager.Instance.PlaySFX(SoundType.SnowBallShoot);
            Random.InitState((int)System.DateTime.Now.Ticks);
            int randomIndex = Random.Range(1, 4);

            switch (randomIndex)
            {
                case 1:
                    SoundManager.Instance.PlaySFX(SoundType.SnowballThrow1);
                    break;

                case 2:
                    SoundManager.Instance.PlaySFX(SoundType.SnowballThrow2);
                    break;

                case 3:
                    SoundManager.Instance.PlaySFX(SoundType.SnowballThrow3);
                    break;
            }

            Debug.Log("Shoot");

            _shooter.Attack();//Well It's actually Shoot() but... lol

            _isReloading = true;
            StartCoroutine("DelayShoot");

            //Debug.Log("Real Shoot");

            _controller.IncreaseSnowGauge(_controller.ShootingSnowGauge);
        }

        public void UpgradeShooter()
        {
            PlayerShooter nextShooter = _shooter.UpgradeShooter();

            if (nextShooter == null) return;

            _shooter = nextShooter;

            _shooter.transform.position = _shooterPos.position;
        }

        private void SetShooter()//Find Shooter at first Time
        {
            _shooter = GetComponentInChildren<PlayerShooter>();

            _shooter.transform.position = _shooterPos.position;
        }

        IEnumerator DelayShoot()
        {
            _isReloading = true;
            //Debug.Log("Start Delay");

            yield return new WaitForSeconds(_controller.ShootingDelay);

            _isReloading = false;
            //Debug.Log("Stop Delay");
        }

        #endregion Shooting

        #region Animator Update

        private void UpdateAnimator()
        {
            Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            _animator.SetFloat("DirX", mouseDir.normalized.x);
            //_animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("Speed", dir.SqrMagnitude());

            _animator.speed = _controller.SpeedIncreaseAmount;
            //Debug.Log(_controller.SpeedIncreaseAmount);
        }

        #endregion Animator Update

        private void SnowGaugeUpdate()//Fuction that will run inside of Update
        {
            _controller.DecreaseSnowGauge(_controller.SnowGaugeDecreaseSpeed * Time.deltaTime);//Decreasing Snow Gauge

        }
    }
}
