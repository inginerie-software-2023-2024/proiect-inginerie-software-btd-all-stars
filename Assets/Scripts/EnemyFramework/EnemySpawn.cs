using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GenericEnemyController enemyPrefab;
    private GenericEnemyController enemy;
    
    public void SpawnEnemy()
    {
        enemy = Instantiate(enemyPrefab);
    }

    public void DespawnEnemy()
    {
        if (enemy != null)
        {
            Destroy(enemy);
        }
    }
}
