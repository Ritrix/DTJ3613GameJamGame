using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private int enemiesToSpawn = 5;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    private bool waveStart;

    private void Start()
    {
        waveStart = true;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        if (waveStart)
        {
            yield return new WaitForSeconds(3.0f);
            waveStart = false;
        }
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        EnemyGeneral enemyAI = newEnemy.GetComponent<EnemyGeneral>();
        if (enemyAI != null)
        {
            enemyAI.SetPlayer(player);
        }
    }
}
