using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SoundManagement;


namespace Game.Player
{
    public class BasicPlayerShooter : PlayerShooter//Shooter that only shot one snow ball
    {
        [SerializeField] Transform _shootPosTransform;//Shooting postion

        public override void Attack()
        {
            base.Attack();

            //PlayerSnowBall snowBallClass = Instantiate(snowBall, _shootPosTransform.position, Quaternion.identity).GetComponent<PlayerSnowBall>();

            //snowBallClass.Init(transform.rotation * Vector2.right);//Give shooting diraction to snowball

            //Debug.Log(_shootPosTransform.position + " " + transform.rotation);

            CreateSnowBall(_shootPosTransform.position, transform.rotation);

        }
    }
}
