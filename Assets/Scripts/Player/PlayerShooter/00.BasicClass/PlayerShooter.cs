using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] protected GameObject snowBall;

        // Update is called once per frame
        protected virtual void Update()
        {
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

        public void UpgradeShooter()
        {

        }
    }
}
