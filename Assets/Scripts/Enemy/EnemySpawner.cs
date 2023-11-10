using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]private BaseEnemy[] enemies;
        [Min(0f)]
        [SerializeField]private float spawnInterval = 2.0f;
        [SerializeField]private Transform target;

        private IEnumerator SpawnEnemies()
        {
            //This condition will change afterwards.
            while(this.enabled)
            {
                int randomIndex = Random.Range(0, enemies.Length);
                BaseEnemy enemy = Instantiate(enemies[randomIndex], transform.position, Quaternion.identity);
                enemy.Target = target;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}
