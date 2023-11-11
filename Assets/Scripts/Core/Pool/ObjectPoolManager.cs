using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Core
{
    [System.Serializable]
    public class PoolObjData
    {
        public PoolObject prefab;
        public int count;
    }

    public class ObjectPoolManager : GenericSingleton<ObjectPoolManager>//Pool Manager with singleton
    {
        [SerializeField,Header("Pool Object List")] private PoolObjData[] poolObjData;

        private Dictionary<int, Queue<PoolObject>> _poolDictionary;

        protected override void Awake()
        {
            base.Awake();

            _poolDictionary = new Dictionary<int, Queue<PoolObject>>();
        }

        private void Start()
        {
            foreach(PoolObjData objData in poolObjData)
            {
                CreatePool(objData.prefab.gameObject, objData.count);
            }
        }

        public void CreatePool(GameObject prefab, int count)
        {
            
            string name = prefab.name + " pool";

            int id = prefab.GetInstanceID();
            if (!_poolDictionary.ContainsKey(id))
            {
                _poolDictionary.Add(id, new Queue<PoolObject>());

                //Set pool object Parent
                GameObject poolParent = new GameObject(name);
                poolParent.transform.parent = transform;

                for(int i = 0; i < count; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    PoolObject poolObject = obj.GetComponent<PoolObject>();

                    poolObject.PrefabID = id;

                    poolObject.transform.parent = poolParent.transform;

                    obj.SetActive(false);

                    _poolDictionary[id].Enqueue(poolObject);

                }
            }
        }

        public GameObject Get(GameObject prefab)
        {
            int id = prefab.GetInstanceID();

            if (!_poolDictionary.ContainsKey(id))
            {
                Debug.LogError(prefab.name + " Pool not found");
                return null;
            }

            PoolObject poolObject;
            if (_poolDictionary.Count != 0)
            {
                poolObject = _poolDictionary[id].Dequeue();
                
            }
            else
            {
                GameObject obj = Instantiate(prefab);
                poolObject = obj.GetComponent<PoolObject>();
            }

            poolObject.Get();

            return poolObject.gameObject;

        }

        public GameObject Get(GameObject prefab,Vector3 pos, Quaternion rot)
        {
            GameObject poolObj = Get(prefab);

            poolObj.transform.position = pos;
            poolObj.transform.rotation = rot;

            return poolObj;
        }


        public void Release(PoolObject poolObject)
        {
            if (poolObject == null)
            {
                Debug.LogError("Null object cannot be returned to pool!!");
                return;
            }


            if (!_poolDictionary.ContainsKey(poolObject.PrefabID))
            {
                Debug.LogError(poolObject.name + " Pool not found!!");
                return;
            }

            Debug.Log("Release obj to Pool");

            poolObject.Release();
            _poolDictionary[poolObject.PrefabID].Enqueue(poolObject);
        }

        public void Release(GameObject poolObject)
        {
            PoolObject obj = poolObject.GetComponent<PoolObject>();

            Release(obj);
        }
    }
}
