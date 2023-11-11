using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] GameObject _nextLevelShooter;

        [SerializeField,Space(10f)] protected GameObject snowBall;

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
