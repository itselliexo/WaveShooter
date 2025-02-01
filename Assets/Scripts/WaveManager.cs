using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] public int currentWave;
    int enemiesToSpawn;
    [SerializeField] int initialEnemies;
    public int enemiesRemaining;
    [SerializeField] float difficaultyRamping = 1.1f;
    [SerializeField] float difficaultyRampingModifier;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] public bool isWaveReady = true;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
   
    void FindSpawnPoints()
    {
        spawnPoints.Clear();

        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (GameObject spawnObject in spawnPointObjects)
        {
            spawnPoints.Add(spawnObject.transform);
        }
    }
    void StartNextWave()
    {   
        currentWave++;
        enemiesToSpawn = currentWave * 5;
        difficaultyRamping = difficaultyRamping += difficaultyRampingModifier;
        enemiesToSpawn = Mathf.FloorToInt(enemiesToSpawn * difficaultyRamping);
        enemiesRemaining = initialEnemies + enemiesToSpawn;
        isWaveReady = false;

        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        for (int h = 0; h <= initialEnemies; h++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenSpawns / currentWave);
        }
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenSpawns / currentWave);
        }
    }
    public void OnEnemiesDefeated()
    { 
        if (enemiesRemaining <= 0)
        {
            isWaveReady = true;
        }
    }
    void Start()
    {
        FindSpawnPoints();
        isWaveReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        OnEnemiesDefeated();
        if (isWaveReady && Input.GetKeyDown(KeyCode.F))
        {
            StartNextWave();
        }
        enemiesRemaining = Mathf.Clamp(enemiesRemaining, 0, int.MaxValue);
    }
}
