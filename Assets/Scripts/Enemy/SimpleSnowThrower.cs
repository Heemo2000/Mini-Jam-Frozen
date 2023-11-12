using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Weapon;
using Game.Core;

namespace Game.Enemy
{
    public class SimpleSnowThrower : BaseEnemy
    {
        [Min(0f)]
        [SerializeField]private float minShootDistance = 5f;
        
        private Gun _gun;

        public float MinShootDistance { get => minShootDistance; }
        public Gun Gun { get => _gun; }

        protected override void Awake() {
            base.Awake();
            _gun = GetComponent<Gun>();
            if (_gun == null) _gun = GetComponentInChildren<Gun>();
        }

        
        // Update is called once per frame
        protected virtual void Update()
        {
            if (!GameMangerObserver.CheckGameMangerWholeStatus()) return;//Change with static checker

            float squareDistanceToTarget = Vector2.SqrMagnitude(base.Target.position - transform.position);
            if(squareDistanceToTarget <= minShootDistance * minShootDistance)
            {
                _gun.Shoot();
            }
        }
        private void OnValidate() {
            if(minShootDistance < base.MinDistanceToTarget)
            {
                minShootDistance = MinDistanceToTarget;
            }
        }
    }
}
