using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int enemiesToSpawn = 5;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    private bool waveStart;

    private int enemiesSpawned;
    private bool isSpawning = false;
    private int enemiesDefeated;

    

    private void Start()
    {
        waveStart = true;
        StartCoroutine(SpawnWave());
        enemiesDefeated = 0;
    }

    public void enemiesKilled()
    {
        enemiesDefeated++;
    }

    private IEnumerator SpawnWave()
    {
        if (waveStart)
        {
            yield return new WaitForSeconds(3.0f);
            waveStart = false;
        }

        enemiesToSpawn = 3 + (GameManager.Instance.currentWave * 2); // 👈 Scale enemies with wave
        enemiesSpawned = 0;
        isSpawning = true;

        while (enemiesSpawned < enemiesToSpawn)
        {
            SpawnEnemy();
            enemiesSpawned++;
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }

        isSpawning = false;
        GameManager.Instance.currentWave++;
        GameManager.Instance.OpenShop();
    }

    private void Update()
    {
        if(enemiesDefeated>= enemiesToSpawn && !isSpawning)
        {
            StartCoroutine(EndWave());
        }
    }


    private IEnumerator EndWave()
    {
        yield return new WaitForSeconds(3.0f);
        GameManager.Instance.currentWave++;
        GameManager.Instance.OpenShop();
    }


    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Give enemy the player reference
        EnemyGeneral enemy = newEnemy.GetComponent<EnemyGeneral>();
        if (enemy != null)
        {
            enemy.SetPlayer(player); // You might need to add player reference inside GameManager!
        }
    }
}
