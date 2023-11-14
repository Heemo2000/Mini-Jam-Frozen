using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]private BaseEnemy[] enemies;
        [Min(0f)]
        [SerializeField]private float maxSpawnInterval = 7.0f;
        [SerializeField] private float minSpawnInterval = 0.5f;
        [SerializeField] float spawnSpeedDecreaseAmount = 1f;

        [SerializeField] int enemySpawnNum = 2;

        [SerializeField,Space(10f)]private Transform target;

        [SerializeField, Space(10f)] Transform spawnPoss;

        private void OnSpawnStart()
        {
            StartCoroutine(SpawnEnemies());
        }
        private IEnumerator SpawnEnemies()
        {
            
            while(this.enabled)
            {
                float spawnInterval = maxSpawnInterval;

                //Debug.Log("Inside spawn coroutine");
                if (GameManager.Instance.GameplayStatus != GameplayStatus.OnGoing)
                {
                    break;
                }
                if(GameManager.Instance.GamePauseStatus == GamePauseStatus.Paused)
                {
                    //Debug.Log("Spawning paused");
                    yield return null;
                }
                else
                {
                    //Debug.Log("Spawned enemy");

                    for (int i = 0; i < enemySpawnNum; i++) SpawnEnemy();

                    yield return new WaitForSeconds(spawnInterval);

                    spawnInterval = spawnInterval > minSpawnInterval ? spawnInterval - spawnSpeedDecreaseAmount : minSpawnInterval;
                }
            }

            //Debug.Log("Stopped spawning");
        }

        void SpawnEnemy()
        {
            int randomIndex = Random.Range(0, enemies.Length);
            Vector3 randomPos = SelectPos();

            BaseEnemy enemy = Instantiate(enemies[randomIndex], randomPos, Quaternion.identity);
            enemy.Target = target;
        }

        Vector3 SelectPos()
        {
            int count = spawnPoss.childCount;

            return spawnPoss.GetChild(Random.Range(0, count)).position;
        }

        private void Start() 
        {
            GameManager.Instance.OnGameplayStart.AddListener(OnSpawnStart);

            if (target == null) target = GameObject.FindWithTag("Player").transform;
        }
    }
}
