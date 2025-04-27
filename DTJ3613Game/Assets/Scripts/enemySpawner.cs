using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject ZealotPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spawnPoints;
    private int enemiesToSpawn = 5;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    private bool waveStart;

    private int enemiesSpawned;
    private bool isSpawning = false;
    public static int enemiesDefeated;



    

    private void Start()
    {
        waveStart = true;
        StartCoroutine(SpawnWave());
        enemiesDefeated = 0;
        UIHandler.instance.SetWaveLabelText("Wave: " + GameManager.Instance.currentWave);
    }

    private IEnumerator SpawnWave()
    {
        if (waveStart)
        {
            yield return new WaitForSeconds(3.0f);
            waveStart = false;
        }

        enemiesToSpawn = 3 + (GameManager.Instance.currentWave * 2); // Scale enemies with wave
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
    }

    private void Update()
    {
        if (enemiesDefeated>= enemiesToSpawn && !isSpawning)
        {
            StartCoroutine(EndWave());
        }
    }

    private void FixedUpdate()
    {
        UIHandler.instance.SetEnemiesRemainingLabelText("Enemies Remaining: " + (enemiesToSpawn - enemiesDefeated));
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
        Vector3 randomOffset = new Vector3(0, Random.Range(-3f, 3f), 0);

        GameObject selectedPrefab;

        if (GameManager.Instance.currentWave > 0)
        {
            // After wave 9, randomly pick between A and B
            if (Random.value > 0.5f)
            {
                selectedPrefab = enemyPrefab;
            }
            else
            {
                selectedPrefab = ZealotPrefab;
            }
        }
        else
        {
            // Before wave 10, always spawn Type A
            selectedPrefab = enemyPrefab;
        }

        GameObject newEnemy = Instantiate(selectedPrefab, spawnPoint.position + randomOffset, Quaternion.identity);

        // Give enemy the player reference
        EnemyGeneral enemy = newEnemy.GetComponent<EnemyGeneral>();
        if (enemy != null)
        {
            enemy.SetPlayer(player);
        }

        // Try to find an EnemyGeneralB script on the spawned enemy
        Zealot enemyZealot = newEnemy.GetComponent<Zealot>();
        if (enemyZealot != null)
        {
            enemyZealot.SetPlayer(player);
        }
    }
}
