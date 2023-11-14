using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    public class HardBullet : Bullet
    {
        [SerializeField] int _hitBySnowBallCount = 2;

        public override void HitSnowBall()
        {
            _hitBySnowBallCount--;

            if(_hitBySnowBallCount <= 0)
            {
                DestroyBullet();
                return;
            }
        }
    }
}
