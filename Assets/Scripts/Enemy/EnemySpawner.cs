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
        [SerializeField]private float spawnInterval = 2.0f;
        [SerializeField]private Transform target;

        private void OnSpawnStart()
        {
            StartCoroutine(SpawnEnemies());
        }
        private IEnumerator SpawnEnemies()
        {
            
            while(this.enabled)
            {
                //Debug.Log("Inside spawn coroutine");
                if(GameManager.Instance.GameplayStatus != GameplayStatus.OnGoing)
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
                    int randomIndex = Random.Range(0, enemies.Length);
                    BaseEnemy enemy = Instantiate(enemies[randomIndex], transform.position, Quaternion.identity);
                    enemy.Target = target;
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            //Debug.Log("Stopped spawning");
        }

        private void Start() 
        {
            GameManager.Instance.OnGameplayStart.AddListener(OnSpawnStart);    
        }
    }
}
