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

            Random.InitState((int)System.DateTime.Now.Ticks);
            int randomIndex = Random.Range(1,4);

            switch(randomIndex)
            {
                case 1: 
                        SoundManager.Instance.PlaySFX(SoundType.SnowballThrow1);                         
                        break;

                case 2: SoundManager.Instance.PlaySFX(SoundType.SnowballThrow2);                         
                        break;

                case 3: SoundManager.Instance.PlaySFX(SoundType.SnowballThrow3);                         
                        break;
            }

        }
    }
}
