using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Game.Core;
namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BaseEnemy : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField]private float moveSpeed = 10f;
        [Min(0f)]
        [SerializeField]private float rotateSpeed = 10f;
        [SerializeField]private bool allowRotation = false;
        [SerializeField]private Transform target;
        [Min(0f)]
        [SerializeField]private float updateInterval = 1.0f;
        [Min(0f)]
        [SerializeField]private float wayPointCheckDistance = 0.5f;
        [Min(0f)]
        [SerializeField]private float minDistanceToTarget = 5f;

        [Header("Flocking(Avoid overlapping) settings:")]

        [Min(2f)]
        [SerializeField]private float neighboursDetectDistance = 5f;

        [Min(0.5f)]
        [SerializeField]private float neighboursDetectInterval = 1.0f;

        [Min(0f)]
        [SerializeField]private float alignmentWeight = 10f;
        
        [Min(0f)]
        [SerializeField]private float cohesionWeight = 10f;

        [Min(0f)]
        [SerializeField]private float separationWeight = 10f;
        
        private List<Rigidbody2D> _detectNeighbours;

        private Seeker _seeker = null;
        private Path _path = null;
        private int _currentIndex = 0;

        private float _currentAngle = 0.0f;
        private Rigidbody2D _enemyRB;
        private Health _health;
        private Collider2D _enemyCollider;

        public Transform Target { get => target; set => target = value; }
        public float MinDistanceToTarget { get => minDistanceToTarget; }

        private Vector2 ComputeAlignment()
        {
            if(_detectNeighbours.Count == 0)
            {
                return Vector2.zero;
            }
            Vector2 result = Vector2.zero;
            
            foreach(var neighbour in _detectNeighbours)
            {
                result += neighbour.velocity;
            }    

            result /= _detectNeighbours.Count;
            result.Normalize();

            return result;
        }

        private Vector2 ComputeCohesion()
        {
            if(_detectNeighbours.Count == 0)
            {
                return Vector2.zero;
            }

            Vector2 result = Vector2.zero;
            result += _enemyRB.velocity;

            result /= _detectNeighbours.Count;

            result = result - _enemyRB.velocity;

            result.Normalize();

            return result;
        }

        private Vector2 ComputeSeparation()
        {
            if(_detectNeighbours.Count == 0)
            {
                return Vector2.zero;
            }

            Vector2 result = Vector2.zero;
            
            foreach(var neighbour in _detectNeighbours)
            {
                result += neighbour.position - _enemyRB.position;
            }

            result *= -1;

            return result;
        }

        private Vector2 ComputeVelocity()
        {
            var alignment = ComputeAlignment();
            var cohesion = ComputeCohesion();
            var separation = ComputeSeparation();

            var result = _enemyRB.velocity;

            result += alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight;

            result.Normalize();

            result *= moveSpeed;
            return result;
        }
        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        private IEnumerator DetectNeigbours()
        {
            while(GameMangerObserver.CheckGameMangerGameStatus())
            {
                _detectNeighbours.Clear();
                int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
                Collider2D[] temp = Physics2D.OverlapCircleAll(_enemyRB.position, neighboursDetectDistance, enemyLayerMask);

                foreach(var collider in temp)
                {
                    if(collider.transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D body))
                    {
                        _detectNeighbours.Add(body);
                    }
                }
                yield return new WaitForSeconds(neighboursDetectInterval);
            }
        }

        private IEnumerator FindPath()
        {
            while(GameMangerObserver.CheckGameMangerGameStatus())
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

        protected virtual void Awake() 
        {
            _seeker = GetComponent<Seeker>();
            _enemyRB = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
            _detectNeighbours = new List<Rigidbody2D>();
            _enemyCollider = GetComponent<Collider2D>();
        }
        
        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            _enemyRB.isKinematic = true;
            _enemyCollider.isTrigger = true;
            //_enemyRB.gravityScale = 0.0f;
            _currentIndex = 0;
            StartCoroutine(FindPath());
            StartCoroutine(DetectNeigbours());
            _health.OnDeath += DestroyEnemy;
            
        }

        // Update is called once per frame
        protected virtual void FixedUpdate()
        {
            if(allowRotation)
            {
                Vector2 targetDirection = (target.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                _currentAngle = Mathf.Lerp(_currentAngle, targetAngle, rotateSpeed * Time.fixedDeltaTime);
                _enemyRB.MoveRotation(_currentAngle);
            }


            if (!GameMangerObserver.CheckGameMangerWholeStatus()) return;//Change with static checker

            float squareDistanceToTarget = Vector2.SqrMagnitude(target.position - transform.position);
            if(_path == null || _currentIndex >= _path.vectorPath.Count || squareDistanceToTarget <= minDistanceToTarget * minDistanceToTarget)
            {    
                return;
            }

            Vector3 wayPoint = _path.vectorPath[_currentIndex];

            Vector2 moveDirection = (wayPoint - transform.position).normalized;
            _enemyRB.MovePosition(_enemyRB.position + (ComputeVelocity() + moveDirection * moveSpeed) * Time.fixedDeltaTime);

            /*
            if(allowRotation)
            {
                float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                _currentAngle = Mathf.Lerp(_currentAngle, targetAngle, rotateSpeed * Time.fixedDeltaTime);
                _enemyRB.MoveRotation(_currentAngle);
            }
            */

            if(Vector2.SqrMagnitude(wayPoint - transform.position) <= wayPointCheckDistance * wayPointCheckDistance)
            {
                _currentIndex++;
            }

        }

        private void OnDestroy() {
            _health.OnDeath -= DestroyEnemy;
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, neighboursDetectDistance);    
        }
    }

}
