using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PoolObject : MonoBehaviour
    {
        private int _prefabID = -1;

        public int PrefabID { get { return _prefabID; } set { _prefabID = value; } }

        public void Get()
        {
            gameObject.SetActive(true);
        }

        public void Release()
        {
            if(_prefabID!= -1)
            {
                //ObjectPoolManager.Instance.Release(this);

                this.transform.position = Vector3.zero;
                this.gameObject.SetActive(false);

                return;
            }

            Destroy(gameObject);
        }
    }
}
