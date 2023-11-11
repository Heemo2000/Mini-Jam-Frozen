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

            StartCoroutine("DestroySnowBall");
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            rb.MovePosition(rb.position + _moveDir * _moveSpeed * Time.deltaTime);
        }

        IEnumerator DestroySnowBall()
        {
            yield return new WaitForSeconds(_destroyTime);

            Destroy(this.gameObject);
        }
    }
}
