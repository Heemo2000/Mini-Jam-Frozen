using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerShooter : MonoBehaviour
    {
        PlayerController _controller;

        [SerializeField] GameObject _nextLevelShooter;

        [SerializeField,Space(10f)] protected GameObject snowBall;

        private void Awake()
        {
            _controller = GetComponentInParent<PlayerController>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (!GameManagerObserver.CheckGameManagerWholeStatus()) return;

            RotateShooter();
        }

        protected void RotateShooter()//Rotate toward mouse
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Quaternion angle = Quaternion.FromToRotation(Vector2.right, (mousePos - (Vector2)transform.position).normalized);

            transform.rotation = angle;

        }

        public virtual void Attack()//Attack Functions
        {

        }

        protected void CreateSnowBall(Vector2 pos, Quaternion rot)
        {
            //Debug.Log("Create Snow Ball");

            PlayerSnowBall snowBallClass;
            if(ObjectPoolManager.Instance) snowBallClass = ObjectPoolManager.Instance.Get(snowBall,pos, Quaternion.Euler(0, 0, 0)).GetComponent<PlayerSnowBall>();
            else snowBallClass = Instantiate(snowBall, pos, Quaternion.Euler(0,0,0)).GetComponent<PlayerSnowBall>();

            //Debug.Log("Create Snow Ball2");

            snowBallClass.Init(rot * Vector2.right,_controller.SnowBallAttackAmount,_controller.SnowBallSizeAmount);

            //Debug.Log("Create Snow Ball3");
        }

        public PlayerShooter UpgradeShooter()
        {
            if (_nextLevelShooter == null) return null;

            Destroy(this.gameObject);

            GameObject shooter = Instantiate(_nextLevelShooter);

            shooter.transform.parent = this.transform.parent;

            return shooter.GetComponent<PlayerShooter>();
        }

        
    }
}
