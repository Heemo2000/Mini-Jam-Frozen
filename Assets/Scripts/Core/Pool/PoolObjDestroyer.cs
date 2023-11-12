using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PoolObjDestroyer : MonoBehaviour
    {
        [SerializeField] float _timer = 2f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DestroyTimer());
        }

        IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(_timer);

            GetComponent<ParticleSystem>().Stop();

            if (ObjectPoolManager.Instance) ObjectPoolManager.Instance.Release(this.gameObject);
            else Destroy(this.gameObject);
        }
    }
}
