using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {

        Rigidbody2D _rb;

        [Header("Movement Setting")]
        [SerializeField] private float _speed = 10f;

        [Header("Shooting Setting")]
        [SerializeField] private float _shootingDelay = 2f;
        [SerializeField] private float _shootingSnowGauge = 10f;

        private PlayerShooter _shooter;//Shoot a snowball with this class

        private bool _isReloading = false;

        [Header("Hp Setting")]//Hp Settings
        [SerializeField] private float _maxHp = 100f;
        [SerializeField] private float _hp;

        public float MaxHp { get => _maxHp; }
        public float Hp {  get => _hp; }

        [Header("Snow Gauge Setting")]//Snow Gauge Setting
        [SerializeField] private float _maxSnowGauge = 100f;
        [SerializeField] private float _snowGauge;

        [SerializeField, Space(5f)] private float _snowGaugeDecreaseSpeed = 2f;

        public float MaxSnowGauge {  get => _maxSnowGauge; }
        public float SnowGauge { get => _snowGauge; }


        private void Awake()
        {
            _hp = _maxHp;

            _snowGauge = 0f;

            _rb = GetComponent<Rigidbody2D>();

            SetShooter();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
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

            _rb.MovePosition(_rb.position + dir * _speed * Time.deltaTime);
        }

        #endregion Movement

        #region Shooting

        private void ShootSnowBall()
        {
            if(_isReloading) return;
            if (!Input.GetMouseButtonDown(0)) return;//Shoot only when click the mouse left button

            _shooter.Attack();//Well It's actually Shoot() but... lol

            ChangSnowGauge(_shootingSnowGauge);
        }

        private void SetShooter()//Find Shooter at first Time
        {
            _shooter = GetComponentInChildren<PlayerShooter>();
        }

        #endregion Shooting

        #region Hp Change Setting

        private void ChangeHp(float amount)
        {
            if (_hp + amount <= 0) return;
            if (_hp + amount >= _maxHp) _hp = _maxHp;

            _hp -= amount;
        }

        public void Damage(float amount)
        {
            ChangeHp(-amount);
        }

        public void RepairHp(float amount)
        {
            ChangeHp(amount);
        }

        #endregion Hp Change Setting

        #region Snow Gauge Setting

        private void ChangSnowGauge(float amount)
        {
            if(_snowGauge + amount <= 0)
            {
                _snowGauge = 0f;
            }

            if(_snowGauge + amount >= _maxSnowGauge)
            {
                _snowGauge = _maxSnowGauge;
            }

            _snowGauge += amount;
        }


        private void SnowGaugeUpdate()//Fuction that will run inside of Update
        {
            ChangSnowGauge(- _snowGaugeDecreaseSpeed * Time.deltaTime);//Decreasing Snow Gauge

        }

        #endregion Snow Gauge Setting
    }
}
