using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameCamera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;

        [SerializeField, Space(5f)] float targetFollowSpeed = 10f;

        // Start is called before the first frame update
        void Start()
        {
            if (target == null) target = GameObject.FindWithTag("Player").transform;//Set target to Player
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (!target) return;

            Vector3 pos = Vector3.Lerp(transform.position, target.position, targetFollowSpeed * Time.deltaTime);
            pos.z = transform.position.z;

            transform.position = pos;
        }
    }
}
