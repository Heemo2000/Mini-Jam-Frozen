using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Game.Core
{
    public class PathGridManager : MonoBehaviour
    {
        [Min(0.5f)]
        [SerializeField]private float scanInterval = 1.0f;
        private AstarPath _pathfinder;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        private IEnumerator ScanAfterSometime()
        {
            //The condition will change.
            while(GameManager.Instance != null && GameManager.Instance.GameplayStatus == GameplayStatus.OnGoing)
            {
                if(GameManager.Instance.GamePauseStatus == GamePauseStatus.UnPaused)
                {
                    _pathfinder.Scan(_pathfinder.graphs);
                    yield return new WaitForSeconds(scanInterval);
                }
                else
                {
                    yield return null;
                }
            }
        }
        private void Awake() {
            _pathfinder = GetComponent<AstarPath>();
        }

        private void Start() {
            StartCoroutine(ScanAfterSometime());
        }


    }
}
