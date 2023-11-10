using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Weapon;
namespace Game.Enemy
{
    public class SimpleSnowThrower : BaseEnemy
    {
        [Min(0f)]
        [SerializeField]private float minShootDistance = 5f;
        
        private Gun _gun;

        protected override void Awake() {
            base.Awake();
            _gun = GetComponent<Gun>();
        }

        // Update is called once per frame
        void Update()
        {
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
