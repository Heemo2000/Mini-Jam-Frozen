using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
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

        private void Awake() 
        {
            _bulletRB = GetComponent<Rigidbody2D>();
            _bulletCollider = GetComponent<CircleCollider2D>();
        }

        private void Start() 
        {
            _bulletRB.isKinematic = true;
            _bulletCollider.isTrigger = true;    
        }

        // Update is called once per frame
        void Update()
        {
            if(_currentTime > destroyTime)
            {
                Destroy(gameObject);
                return;
            }

            _currentTime += Time.deltaTime;
        }

        private void FixedUpdate() 
        {
            _bulletRB.MovePosition(transform.position + transform.right * moveSpeed * Time.fixedDeltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other) 
        {
            int layerMask = 1 << other.gameObject.layer;

            if((layerMask & detectMask.value) != 0)
            {
                Health health = other.transform.GetComponent<Health>();
                health?.OnHealthDamaged?.Invoke(damage);
                //Instantiate(destroyEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Physics2D.IgnoreCollision(_bulletCollider, other);
            }
        }
    }
}