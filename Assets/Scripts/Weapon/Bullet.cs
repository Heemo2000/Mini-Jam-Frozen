using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.SoundManagement;
using Random = UnityEngine.Random;

namespace Game.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Bullet : MonoBehaviour
    {   
        [SerializeField]protected float moveSpeed = 10f;
        [SerializeField]private float destroyTime = 10f;
        [Min(0f)]
        [SerializeField]protected float damage = 10f;
        [SerializeField]protected LayerMask detectMask;
        [SerializeField]protected ParticleSystem destroyEffect;
       
        private float _currentTime = 0.0f;

        private Rigidbody2D _bulletRB;
        private CircleCollider2D _bulletCollider;

        Vector3 _dir;

        bool _hit;

        private void Awake() 
        {
            _bulletRB = GetComponent<Rigidbody2D>();
            _bulletCollider = GetComponent<CircleCollider2D>();

            _hit = false;
        }

        private void Start() 
        {
            _bulletRB.isKinematic = true;
            _bulletCollider.isTrigger = true;

            _dir = transform.right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if(!GameManagerObserver.CheckGameManagerWholeStatus())
            {
                return;
            }
            if(_currentTime > destroyTime)
            {
                Destroy(gameObject);
                return;
            }

            _currentTime += Time.deltaTime;
        }

        private void FixedUpdate() 
        {
            if(!GameManagerObserver.CheckGameManagerWholeStatus())
            {
                return;
            }
            _bulletRB.MovePosition(transform.position + _dir * moveSpeed * Time.fixedDeltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other) 
        {
            if (_hit) return;

            int layerMask = 1 << other.gameObject.layer;

            if((layerMask & detectMask.value) != 0)
            {
                Health health = other.transform.GetComponent<Health>();
                health?.OnHealthDamaged?.Invoke(damage);
                
                Random.InitState((int)System.DateTime.Now.Ticks);
                int randomIndex = Random.Range(1,4);

                switch(randomIndex)
                {
                    case 1: 
                            SoundManager.Instance.PlaySFX(SoundType.HitBySnowball1);                         
                            break;

                    case 2: SoundManager.Instance.PlaySFX(SoundType.HitBySnowball2);                         
                            break;

                    case 3: SoundManager.Instance.PlaySFX(SoundType.HitBySnowball3);                         
                            break;
                }
                
                //Instantiate(destroyEffect, transform.position, Quaternion.identity);
                if (ObjectPoolManager.Instance) ObjectPoolManager.Instance.Get(destroyEffect.gameObject, transform.position, Quaternion.identity);

                _hit = true;
                Destroy(gameObject);
            }
            else
            {
                Physics2D.IgnoreCollision(_bulletCollider, other);
            }
        }
    }
}
