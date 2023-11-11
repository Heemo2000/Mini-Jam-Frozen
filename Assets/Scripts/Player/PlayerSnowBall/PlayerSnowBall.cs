using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
namespace Game.Player
{
    public class PlayerSnowBall : MonoBehaviour//Snow ball class for Player
    {
        private Rigidbody2D rb;

        private Collider2D _collider;

        [SerializeField] float _moveSpeed = 10f;

        [SerializeField, Space(10f)] float _damage = 5f;
        [SerializeField, Space(10f)] float _destroyTime = 3f;
        [SerializeField]private LayerMask enemyMask;
        private Vector2 _moveDir;

        public void Init(Vector2 moveDir, float attackAmout, float sizeAmout)
        {
            _moveDir = moveDir.normalized;

            //Debug.Log(_moveDir + "Snow ball start Moveing ");

            rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();

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

        private void OnCollisionEnter2D(Collision2D other) 
        {
            int layerMask = 1 << other.gameObject.layer;

            if((layerMask & enemyMask.value) != 0)
            {
                Health health = other.transform.GetComponent<Health>();
                health?.OnHealthDamaged?.Invoke(_damage);
                ScoreManager.Instance.OnScoreIncreased?.Invoke(_damage);
                Destroy(gameObject);
            }            
            else
            {
                Physics2D.IgnoreCollision(_collider, other.collider);
            }
            
        }
    }
}
