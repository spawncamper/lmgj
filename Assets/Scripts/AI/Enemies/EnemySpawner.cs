using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 5f;
    [SerializeField] float enemyDestructionDelay = 30f;
    [SerializeField] GameObject enemyPrefab;
    bool isSpawning = false;

    public delegate void EnemySpawned();
    public static event EnemySpawned EnemySpawnedEvent;

    void OnEnable()
    {
//        GameManager.RoundStartedEvent += StartSpawning;
        GameManager.RoundStartedEvent += SpawnOnce;
    }

    void OnDisable()
    {
//        GameManager.RoundStartedEvent -= StartSpawning;
        GameManager.RoundStartedEvent -= SpawnOnce;
    }

    IEnumerator StartSpawningCoroutine()
    {
        while (isSpawning == true)
        { 
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void StartSpawning()
    {
        isSpawning = true;

        StartCoroutine(StartSpawningCoroutine());
    }

    void SpawnOnce()
    {
        SpawnEnemy();
    }


    void SpawnEnemy()
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, Quaternion.identity) as GameObject;
        enemyInstance.transform.parent = gameObject.transform;

        DestroyObject(enemyInstance, enemyDestructionDelay);

        if (EnemySpawnedEvent != null)
            EnemySpawnedEvent();
    }
}
