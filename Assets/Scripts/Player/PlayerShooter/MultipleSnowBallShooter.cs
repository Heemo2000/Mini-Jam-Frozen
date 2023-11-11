using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class MultipleSnowBallShooter : PlayerShooter
    {
        [SerializeField] List<Transform> _shootPoss = new List<Transform>();

        public override void Attack()
        {
            base.Attack();

            PlayerSnowBall snowBallClass;
            foreach (Transform pos in _shootPoss)
            {
                snowBallClass = Instantiate(snowBall, pos.position, Quaternion.identity).GetComponent<PlayerSnowBall>();

                snowBallClass.Init(pos.rotation * Vector2.right);//Give shoot direction with anchor rotation
            }
        }
    }
}
