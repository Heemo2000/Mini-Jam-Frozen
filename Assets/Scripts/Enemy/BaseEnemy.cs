using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Game.Core;
using Game.SoundManagement;

using Random = UnityEngine.Random;
namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BaseEnemy : MonoBehaviour
    {
        private const int MaxNeighboursDetect = 8; 
        [Min(0f)]
        [SerializeField]private float moveSpeed = 10f;
        [SerializeField] private float freezeMoveSpeed = 0.5f;
        private float _curMoveSpeed = 0f;

        [Min(0f)]
        [SerializeField,Space(10f)]private float rotateSpeed = 10f;
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

        private Vector2 _alignmentVelocity = Vector2.zero;
        private Vector2 _cohesionVelocity = Vector2.zero;

        private Vector2 _separationVelocity = Vector2.zero;

        public Transform Target { get => target; set => target = value; }
        public float MinDistanceToTarget { get => minDistanceToTarget; }

        //For Animation and Effect
        [SerializeField,Space(10f)] private float _freezeSpeed = 1f;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        bool _isMoving = false;

        private Collider2D[] _temp;

        private Vector2 ComputeVelocity()
        {
            
            var result = _enemyRB.velocity;

            result += _alignmentVelocity * alignmentWeight + _cohesionVelocity * cohesionWeight + _separationVelocity * separationWeight;
            result.Normalize();

            result *= moveSpeed;
            return result;
        }
        private void DestroyEnemy()
        {
            animator.speed = 0f;
            spriteRenderer.material.SetFloat("_FreezeValue", 1f);

            Collider2D[] colls = GetComponents<Collider2D>();
            foreach (Collider2D coll in colls)
                coll.enabled = false;

            this.enabled = false;
            this.GetComponent<Seeker>().enabled = false;
            Destroy(this.GetComponent<Seeker>());

            EnemyDeathCounter.Instance.IncreaseCount();

            /*
            Random.InitState((int)System.DateTime.Now.Ticks);
            int randomIndex = Random.Range(1,5);

            switch(randomIndex)
            {
                case 1: 
                        SoundManager.Instance.PlaySFX(SoundType.DeathHurt1);                         
                        break;

                case 2: SoundManager.Instance.PlaySFX(SoundType.DeathHurt2);                         
                        break;

                case 3: SoundManager.Instance.PlaySFX(SoundType.DeathHurt3);                         
                        break;
                
                case 4: SoundManager.Instance.PlaySFX(SoundType.DeathHurt4);                         
                        break;
            }
            */
        }

        private IEnumerator DetectNeigbours()
        {
            while(GameManagerObserver.CheckGameManagerGameStatus())
            {
                _detectNeighbours.Clear();
                int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
                int count = Physics2D.OverlapCircleNonAlloc(_enemyRB.position, neighboursDetectDistance,_temp, enemyLayerMask);
                for(int i = 0; i < count; i++)
                {
                    Collider2D collider = _temp[i];
                    if(collider.transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D body))
                    {
                        _detectNeighbours.Add(body);
                    }
                }

                _alignmentVelocity = Vector2.zero;

                foreach(var neighbour in _detectNeighbours)
                {
                    _alignmentVelocity += neighbour.velocity;
                }    

                _alignmentVelocity /= _detectNeighbours.Count;
                _alignmentVelocity.Normalize();

                _cohesionVelocity = Vector2.zero;
                _cohesionVelocity += _enemyRB.velocity;

                _cohesionVelocity /= _detectNeighbours.Count;

                _cohesionVelocity = _cohesionVelocity - _enemyRB.velocity;

                _cohesionVelocity.Normalize();

                _separationVelocity = Vector2.zero;
            
                foreach(var neighbour in _detectNeighbours)
                {
                    _separationVelocity += neighbour.position - _enemyRB.position;
                }

                _separationVelocity *= -1;

                yield return new WaitForSeconds(neighboursDetectInterval);
            }
        }

        private IEnumerator FindPath()
        {
            while(GameManagerObserver.CheckGameManagerGameStatus())
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

        protected void UpdateAnimator()
        {
            float freezeValue = Mathf.Lerp(spriteRenderer.material.GetFloat("_FreezeValue"), 1f - _health.CurrentAmount / _health.MaxHealth, _freezeSpeed * Time.deltaTime);
            spriteRenderer.material.SetFloat("_FreezeValue", freezeValue);

            animator.SetBool("IsMoving", _isMoving);

            Vector2 mouseDir = target.position - transform.position;
            animator.SetFloat("DirX", mouseDir.normalized.x);

            animator.speed = 1 - freezeValue;
        }

        protected virtual void Awake() 
        {
            _seeker = GetComponent<Seeker>();
            _enemyRB = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
            _detectNeighbours = new List<Rigidbody2D>();
            _enemyCollider = GetComponent<Collider2D>();

            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            _temp = new Collider2D[MaxNeighboursDetect];
        }
        
        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            _enemyRB.isKinematic = true;
            //_enemyCollider.isTrigger = true;
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

            if (!GameManagerObserver.CheckGameManagerWholeStatus()) return;//Change with static checker

            float squareDistanceToTarget = Vector2.SqrMagnitude(target.position - transform.position);
            if(_path == null || _currentIndex >= _path.vectorPath.Count || squareDistanceToTarget <= minDistanceToTarget * minDistanceToTarget)
            {
                
                if(squareDistanceToTarget <= minDistanceToTarget * minDistanceToTarget)
                    _isMoving = false;
                else
                    _isMoving = true;
                

                return;
            }
            _isMoving = true;

            _curMoveSpeed = Mathf.Lerp(freezeMoveSpeed, moveSpeed, _health.CurrentAmount / _health.MaxHealth);

            Vector3 wayPoint = _path.vectorPath[_currentIndex];

            Vector2 moveDirection = (wayPoint - transform.position).normalized;
            _enemyRB.MovePosition(_enemyRB.position + (ComputeVelocity() + moveDirection * _curMoveSpeed) * Time.fixedDeltaTime);

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
