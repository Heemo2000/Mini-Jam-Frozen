using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy
{
    public class IntervalSnowBallThrower : SimpleSnowThrower
    {
        [Min(0.5f)]
        [SerializeField]private float shootInterval = 2.0f;

        private bool _allowShooting = false;
        
        private float _currentTime = 0.0f;

        protected override void Update()
        {
            if (!GameMangerObserver.CheckGameMangerWholeStatus()) return;//Change with static checker

            float squareDistanceToTarget = Vector2.SqrMagnitude(base.Target.position - transform.position);
            if(squareDistanceToTarget <= base.MinShootDistance * base.MinShootDistance)
            {
                if(_allowShooting)
                {
                    base.Gun.Shoot();
                }

                if(_currentTime > shootInterval)
                {
                    _allowShooting = !_allowShooting;
                    _currentTime = 0.0f;
                }
                else
                {
                    _currentTime += Time.deltaTime;
                }
                
            }

        }
    }
}
