using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SoundManagement;
using Game.Weapon;

namespace Game.Player
{
    public class PlayerSnowBall : MonoBehaviour//Snow ball class for Player
    {
        private Rigidbody2D rb;

        private Collider2D _collider;

        [SerializeField] float _moveSpeed = 10f;

        [SerializeField, Space(10f)] float _damage = 5f;
        [SerializeField, Space(10f)] float _destroyTime = 3f;

        [SerializeField, Space(10f)] private LayerMask enemyMask;
        [SerializeField] LayerMask enemySnowballMask;

        [SerializeField, Header("Effect")] GameObject _destroyParticle;
        private Vector2 _moveDir;

        float _attackAmount, _sizeAmount;

        bool _hit = false;

        public void Init(Vector2 moveDir, float attackAmount, float sizeAmount)
        {
            _moveDir = moveDir.normalized;

            //Debug.Log(_moveDir + "Snow ball start Moveing ");

            rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();

            _damage *= attackAmount;
            transform.localScale = transform.localScale * sizeAmount;

            _attackAmount = attackAmount;
            _sizeAmount = sizeAmount;

            _hit = false;

            StartCoroutine("DestroyTimer");
        }

        private void FixedUpdate()
        {
            if (!GameManagerObserver.CheckGameManagerWholeStatus()) return;

            Move();
        }

        private void Move()
        {
            rb.MovePosition(rb.position + _moveDir * _moveSpeed * Time.deltaTime);
        }

        private void DestroySnowBall()
        {
            if (_hit) return;
            _hit = true;
            //Debug.Log("Destroy snowball");

            //Set setting to default for pooling
            _damage /= _attackAmount;
            transform.localScale /= _sizeAmount;

            if (ObjectPoolManager.Instance) ObjectPoolManager.Instance.Get(_destroyParticle,transform.position,Quaternion.identity);
            
            Random.InitState((int)System.DateTime.Now.Ticks);
            int randomIndex = Random.Range(1, 4);

            switch (randomIndex)
            {
                case 1:
                    SoundManager.Instance.PlaySFX(SoundType.HitBySnowball1);
                    break;

                case 2:
                    SoundManager.Instance.PlaySFX(SoundType.HitBySnowball2);
                    break;

                case 3:
                    SoundManager.Instance.PlaySFX(SoundType.HitBySnowball3);
                    break;
            }
            
            //Debug.Log("Hit");

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
            if (_hit) return;
            int layerMask = 1 << other.gameObject.layer;

            if((layerMask & enemyMask.value) != 0)
            {
                Health health = other.transform.GetComponent<Health>();
                health?.OnHealthDamaged?.Invoke(_damage);

                if(ScoreManager.Instance) ScoreManager.Instance.OnScoreIncreased?.Invoke(_damage);

                ScoreManager.Instance.OnScoreIncreased?.Invoke(_damage);
                DestroySnowBall();
                
            }            
            else
            {
                Physics2D.IgnoreCollision(_collider, other.collider);
            }
            
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            int layerMask = 1 << other.gameObject.layer;

            if((layerMask & enemyMask.value) != 0)
            {
                Health health = other.transform.GetComponent<Health>();
                health?.OnHealthDamaged?.Invoke(_damage);

                if (ScoreManager.Instance) ScoreManager.Instance.OnScoreIncreased?.Invoke(_damage);

                ScoreManager.Instance.OnScoreIncreased?.Invoke(_damage);
                DestroySnowBall();
                
            }
            else if((layerMask & enemySnowballMask.value) != 0)
            {
                other.gameObject.GetComponent<Bullet>().HitSnowBall();
                DestroySnowBall();
            }
            else
            {
                Physics2D.IgnoreCollision(_collider, other);
            }            
        }
    }
}
