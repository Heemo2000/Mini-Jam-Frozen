using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerSnowBall : MonoBehaviour//Snow ball class for Player
    {
        private Rigidbody2D rb;

        [SerializeField] float _moveSpeed = 10f;

        [SerializeField, Space(5f)] float _destroyTime = 3f;

        private Vector2 _moveDir;

        public void Init(Vector2 moveDir)
        {
            _moveDir = moveDir.normalized;

            rb = GetComponent<Rigidbody2D>();

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
