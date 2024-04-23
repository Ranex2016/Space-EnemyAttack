using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefabs;

    [SerializeField]
    private GameObject _asteroidPrefabs;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    public GameObject[] _powers;
    private bool _stopSpawning = false;

    private void Start() {
        StartSpawnManager();
    }

    public void StartSpawnManager()
    {
        //запуск корутины
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnAsteroidRoutin());
    }

    // Корутина повтора
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(Random.Range(7, 5));
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefabs, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(2, 5));
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(Random.Range(7, 15));
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powers[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(7, 15));
        }
    }

    IEnumerator SpawnAsteroidRoutin()
    {
        yield return new WaitForSeconds(10f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
            GameObject asteroid = Instantiate(_asteroidPrefabs, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
