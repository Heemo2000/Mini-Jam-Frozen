using Game.StateMachineManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;


namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        //State Setting
        private StateMachine _playerStateMachine;

        private PlayerDefaultState _defaultState;
        private PlayerFrozenState _frozenState;

        //Rigidbody2D _rb;

        [Header("Movement Setting")]
        [SerializeField] private float _speed = 10f;

        public float FinalSpeed { //Calculating Final Speed
            get {
                return _speed * (100f + _playerSpeedAmount) / 100f * (100f - _snowGaugeSpeedDecreaseAmount * _snowGauge / _maxSnowGauge) / 100f;
            } 
        }

        [Header("Shooting Setting")]
        [SerializeField] private float _shootingDelay = 2f;
        [SerializeField] private float _shootingSnowGauge = 10f;

        public float ShootingDelay { get => _shootingDelay; }
        public float ShootingSnowGauge { get => _shootingSnowGauge; }

        

        [Header("Hp Setting")]//Hp Settings
        [SerializeField] private float _maxHp = 100f;
        [SerializeField] private float _hp;

        public float MaxHp { get => _maxHp; }
        public float Hp {  get => _hp; }

        [Header("Snow Gauge Setting")]//Snow Gauge Setting
        [SerializeField] private float _maxSnowGauge = 100f;
        [SerializeField] private float _snowGauge;

        [SerializeField, Space(5f)] private float _snowGaugeDecreaseSpeed = 2f;

        public float SnowGaugeDecreaseSpeed { get => _snowGaugeDecreaseSpeed; }

        public float MaxSnowGauge { get => _maxSnowGauge; }
        public float SnowGauge { get => _snowGauge; }


        [Header("Froze Setting")]
        [SerializeField] private float _freezeTime = 2f;

        public float FreezeTime { get => _freezeTime; }
        public bool IsFrozen { get; set; } = false;

        [Header("Power Up Setting (%)")]
        [SerializeField] private float _snowBallAttackAmount = 0f;
        [SerializeField] private float _snowBallSizeAmount = 0f;
        [SerializeField] private float _playerSpeedAmount = 0f;

        [Space(5f)]
        [SerializeField] private float _snowGaugeIncreaseAmount = 0f;
        [SerializeField] private float _snowGaugeDecreaseAmount = 0f;
        [SerializeField] private float _snowGaugeSpeedDecreaseAmount = 0f;//It's Decreaseing Player Speed

        public float SnowBallAttackAmount { get => (100f + _snowBallAttackAmount) / 100f; }
        public float SnowBallSizeAmount { get => (100f + _snowBallSizeAmount) / 100f; }


        private void Awake()
        {
            _hp = _maxHp;

            _snowGauge = 0f;

            //_rb = GetComponent<Rigidbody2D>();

            SetStateMachine();

        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (GameMangerObserver.CheckGameMangerStatus()) return;

            //Debug.Log("Update");
            _playerStateMachine.OnUpdate();

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.E)) UpgradeShooter();//Testing Upgrade System
#endif
        }

        private void FixedUpdate()
        {
            if (GameMangerObserver.CheckGameMangerStatus()) return;

            _playerStateMachine.OnFixedUpdate();//Run Update in Fixed Update for RigidBody
        }

        private void SetStateMachine()//Set the player State and State Machine
        {
            _playerStateMachine = new StateMachine();

            //_defaultState = new PlayerDefaultState(this);
            //_frozenState = new PlayerFrozenState(this);

            //Use State as a Component
            _defaultState = GetComponent<PlayerDefaultState>();
            _frozenState = GetComponent<PlayerFrozenState>();

            _playerStateMachine.AddTransition(_defaultState, _frozenState, () => IsFrozen == true);//Change State to Frozen when isFrozen is true
            _playerStateMachine.AddTransition(_frozenState, _defaultState, () => IsFrozen == false);

            _playerStateMachine?.SetState(_defaultState);
        }

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

        private void ChangeSnowGauge(float amount)
        {
            if(_snowGauge + amount <= 0)
            {
                _snowGauge = 0f;
            }
            else if(_snowGauge + amount >= _maxSnowGauge)
            {
                _snowGauge = _maxSnowGauge;
                IsFrozen = true;//Change to frozen State
            }
            else
                _snowGauge += amount;
        }

        public void IncreaseSnowGauge(float amount)
        {
            amount *= (100f + _snowGaugeIncreaseAmount)/100f;

            ChangeSnowGauge(amount);
        }

        public void DecreaseSnowGauge(float amount)
        {
            amount *= (100f + _snowGaugeDecreaseAmount) / 100f;

            ChangeSnowGauge(-amount);
        }

        public void SetSnowGauge(float snowGauge)
        {
            _snowGauge = snowGauge;
        }

        #endregion Snow Gauge Setting

        #region Power Up Setting

        public void UpgradeShooter()//You can call this function to Upgrade Shooter
        {
            _defaultState.UpgradeShooter();
        }

        public class PowerUpData//Use this class for powerups
        {
            public bool UpgradeShooter = false;

            public float SnowBallAttackAmount = 0f;
            public float SnowBallSizeAmount = 0f;
            public float PlayerSpeedAmount = 0f;

            public float SnowGaugeIncreaseAmount = 0f;
            public float SnowGaugeDecreaseAmount = 0f;
            public float SnowGaugeSpeedDecreaseAmount = 0f;
        }

        public void PowerUp(PowerUpData data)
        {
            if (data.UpgradeShooter) UpgradeShooter();

            _snowBallAttackAmount += data.SnowBallAttackAmount;
            _snowBallSizeAmount += data.SnowBallSizeAmount;
            _playerSpeedAmount += data.PlayerSpeedAmount;

            _snowGaugeIncreaseAmount += data.SnowGaugeIncreaseAmount;
            _snowGaugeDecreaseAmount += data.SnowGaugeDecreaseAmount;
            _snowGaugeSpeedDecreaseAmount += data.SnowGaugeSpeedDecreaseAmount;
        }


        #endregion Power Up Setting

    }
}
