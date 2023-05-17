using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnableObject
{
    public GameObject prefab;
    [Range(0, 1)] public float probability;
}

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private SpawnableObject[] _enemyPrefabs;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private SpawnableObject[] _powerUps;
    private bool _stopSpawning = false;
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("The UIManager is Null");
        }
    }


    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(4.0f);

        for (int wave = 1; wave <= 6; wave++)
        {
            Debug.Log("We are in Wave: " + wave);
            _uiManager.ShowWaveNumber(wave);

            int enemiesToSpawn = wave < 6 ? wave : Random.Range(1, 4); // set the number of enemies to spawn based on the current wave
            int enemiesSpawned = 0;

            // Get the enemy probabilities
            float[] enemyProbabilities = new float[_enemyPrefabs.Length];
            for (int i = 0; i < _enemyPrefabs.Length; i++)
            {
                enemyProbabilities[i] = _enemyPrefabs[i].probability;
            }

            while (enemiesSpawned < enemiesToSpawn)
            {
                Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 6.73f, 0);
            
                int randomEnemy = GetRandomIndexWithProbabilities(enemyProbabilities);
            
                GameObject newEnemy = Instantiate(_enemyPrefabs[randomEnemy].prefab, positionToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                enemiesSpawned++;

                yield return new WaitForSeconds(1.0f);
            }

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0); // wait until all enemies are dispatched

            if (wave < 6)
            {
                yield return new WaitForSeconds(5.0f);
            }
            else
            {
                SpawnBoss();
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 _positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            // Get the power-up probabilities
            float[] powerUpProbabilities = new float[_powerUps.Length];
            for (int i = 0; i < _powerUps.Length; i++)
            {
                powerUpProbabilities[i] = _powerUps[i].probability;
            }

            int _randomPowerUp = GetRandomIndexWithProbabilities(powerUpProbabilities);

            Instantiate(_powerUps[_randomPowerUp].prefab, _positionToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 11));
        }
    }


    private int GetRandomIndexWithProbabilities(float[] probabilities)
    {
        float total = 0;
        float[] cumulativeProbabilities = new float[probabilities.Length];

        for (int i = 0; i < probabilities.Length; i++)
        {
            total += probabilities[i];
            cumulativeProbabilities[i] = total;
        }

        float randomNumber = Random.Range(0, total);
        int index = Array.FindIndex(cumulativeProbabilities, x => x >= randomNumber);
        return index;
    }

    public void SpawnBoss()
    {
        Vector3 positionToSpawn = new Vector3(0, 6.73f, 0);
        GameObject boss = Instantiate(_bossPrefab, positionToSpawn, Quaternion.identity);
        boss.transform.parent = _enemyContainer.transform;
    }


    public void OnPlayerDeath()
    {
        Debug.Log("Death Called Stop Spawning!");
        _stopSpawning = true;
    }
    
    

}
