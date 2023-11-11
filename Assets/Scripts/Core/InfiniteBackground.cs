using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class InfiniteBackground : MonoBehaviour
    {
        [SerializeField]private Transform followTransform;
        [SerializeField]private SpriteRenderer graphic;
        [SerializeField]private Vector2 tileMultiplier;

        [SerializeField]private Vector3 offset;
        private float _originalWidth = 0.0f;
        private float _originalHeight = 0.0f;


        // Start is called before the first frame update
        void Start()
        {
            _originalWidth = graphic.sprite.bounds.size.x;
            _originalHeight = graphic.sprite.bounds.size.y;

            transform.position = followTransform.position + offset;
        }

        // Update is called once per frame
        void Update()
        {
            float squareDistance = Vector2.SqrMagnitude(followTransform.position - transform.position);
            if((squareDistance > _originalWidth * _originalWidth) || (squareDistance > _originalHeight * _originalHeight))
            {
                transform.position = followTransform.position + offset;
            }
        }
    }
}
