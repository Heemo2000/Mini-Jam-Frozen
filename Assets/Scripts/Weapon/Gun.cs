using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SoundManagement; 

namespace Game.Weapon
{
    public class Gun : MonoBehaviour
    {
        [SerializeField]private Transform firePoint;
        [Min(0)]
        [SerializeField]private int fireRate = 10;
        [SerializeField]private Bullet bulletPrefab;
        private float _currentTime = 0.0f;
        public Action OnBulletFired;

        Transform rotateTarget;

        private void Awake()
        {
            rotateTarget = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            Quaternion angle = Quaternion.FromToRotation(Vector2.right, (Vector2)(rotateTarget.position - transform.position).normalized);

            transform.rotation = angle;
        }

        public void Shoot()
        {
            if(_currentTime < Time.time)
            {
                OnBulletFired?.Invoke();
                _currentTime = Time.time + 1.0f/(float)fireRate;
            }
        }

        protected virtual void PlayShootSound()
        {
            //SoundManager.Instance.PlaySFX(SoundType.BulletShoot);
        }

        private void FireBullet()
        {
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.transform.right = firePoint.right;    
        }
        private void Start() 
        {
            OnBulletFired += FireBullet;
            OnBulletFired += PlayShootSound;
        }

        private void OnDestroy() 
        {
            OnBulletFired -= FireBullet;
            OnBulletFired -= PlayShootSound;    
        }
    }
}
