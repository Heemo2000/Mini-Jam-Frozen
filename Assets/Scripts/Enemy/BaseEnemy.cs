using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField]private float moveSpeed = 10f;
        
        [SerializeField]private Transform target;
        [Min(0f)]
        [SerializeField]private float updateInterval = 1.0f;
        [Min(0f)]
        [SerializeField]private float wayPointCheckDistance = 0.5f;


        [Min(0f)]
        [SerializeField]private float minDistanceToTarget = 5f;
        private Seeker _seeker = null;
        private Path _path = null;
        private int _currentIndex = 0;
        private Rigidbody2D _enemyRB;
        
        private IEnumerator FindPath()
        {
            while(this.enabled)
            {
                float squareDistanceToTarget = Vector2.SqrMagnitude(target.position - transform.position);

                if((squareDistanceToTarget <= minDistanceToTarget * minDistanceToTarget) && !_seeker.IsDone())
                {
                    yield return null;
                }
                else
                {
                    _seeker.StartPath(transform.position, target.position, OnPathComplete);
                    yield return new WaitForSeconds(updateInterval);
                }
            }
        }

        private void OnPathComplete(Path path)
        {
            
            if(path != null && !path.error)
            {
                _path = path;
                _currentIndex = 0;
            }
            else
            {
                Debug.LogError("Error while setting a path: \n" + _path.errorLog);
            }
        }

        private void Awake() 
        {
            _seeker = GetComponent<Seeker>();
            _enemyRB = GetComponent<Rigidbody2D>();
        }
        
        
        // Start is called before the first frame update
        void Start()
        {
            _enemyRB.isKinematic = true;
            _currentIndex = 0;
            StartCoroutine(FindPath());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float squareDistanceToTarget = Vector2.SqrMagnitude(target.position - transform.position);
            if(_path == null || _currentIndex >= _path.vectorPath.Count || squareDistanceToTarget <= minDistanceToTarget * minDistanceToTarget)
            {
                return;
            }

            Vector3 wayPoint = _path.vectorPath[_currentIndex];

            Vector2 moveDirection = (wayPoint - transform.position).normalized;
            _enemyRB.MovePosition(_enemyRB.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            
            if(Vector2.SqrMagnitude(wayPoint - transform.position) <= wayPointCheckDistance * wayPointCheckDistance)
            {
                _currentIndex++;
            }

        }
    }

}
