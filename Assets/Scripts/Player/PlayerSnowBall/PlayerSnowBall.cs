using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerSnowBall : MonoBehaviour//Snow ball class for Player
    {
        private Rigidbody2D rb;

        [SerializeField] float _moveSpeed = 10f;

        [SerializeField, Space(10f)] float _damage = 5f;

        [SerializeField, Space(10f)] float _destroyTime = 3f;

        private Vector2 _moveDir;

        public void Init(Vector2 moveDir, float attackAmout, float sizeAmout)
        {
            _moveDir = moveDir.normalized;

            //Debug.Log(_moveDir + "Snow ball start Moveing ");

            rb = GetComponent<Rigidbody2D>();

            _damage *= attackAmout;
            transform.localScale = transform.localScale * sizeAmout;

            StartCoroutine("DestroyTimer");
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            rb.MovePosition(rb.position + _moveDir * _moveSpeed * Time.deltaTime);
        }

        private void DestroySnowBall()
        {
            Debug.Log("Destroy snowball");

            if (ObjectPoolManager.Instance) ObjectPoolManager.Instance.Release(this.gameObject);
            else Destroy(this.gameObject);
        }

        IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(_destroyTime);

            DestroySnowBall();
        }
    }
}
