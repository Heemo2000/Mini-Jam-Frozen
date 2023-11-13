using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SoundManagement; 
using Game.Core;

using Random = UnityEngine.Random;

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
            if(!GameManagerObserver.CheckGameManagerWholeStatus())
            {
                return;
            }
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
            Random.InitState((int)System.DateTime.Now.Ticks);
            int randomIndex = Random.Range(1,4);

                switch(randomIndex)
                {
                    case 1: 
                            SoundManager.Instance.PlaySFX(SoundType.SnowballThrow1);                         
                            break;

                    case 2: SoundManager.Instance.PlaySFX(SoundType.SnowballThrow2);                         
                            break;

                    case 3: SoundManager.Instance.PlaySFX(SoundType.SnowballThrow3);                         
                            break;
                }
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
