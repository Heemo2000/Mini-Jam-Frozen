using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyHealth : Health
    {
        SpriteRenderer spriteRenderer;

        float effectSpeed = 6f;

        float hitValue = 0f;
        // Start is called before the first frame update
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            hitValue = 0f;
        }

        private void Update()
        {
            spriteRenderer.material.SetFloat("_HitValue", hitValue);

            hitValue -= effectSpeed * Time.deltaTime;
            hitValue = hitValue <= 0f ? 0f : hitValue;
        }

        protected override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);

            hitValue = 1f;
        }
    }
}
