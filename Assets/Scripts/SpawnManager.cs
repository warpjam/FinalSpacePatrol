using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUps;
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
            while (enemiesSpawned < enemiesToSpawn)
            {
                Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 6.73f, 0);
                int randomEnemy = Random.Range(0, 2);
                GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], positionToSpawn, Quaternion.identity);
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
                Debug.Log("The Boss Should Spawn Now");
                // spawn the boss enemy
                /*Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 6.73f, 0);
                GameObject bossEnemy = Instantiate(_bossEnemyPrefab, positionToSpawn, Quaternion.identity);
                bossEnemy.transform.parent = _enemyContainer.transform;*/
            }
        }
    }



    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 _positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int _randomPowerUp = Random.Range(0, 7);
            if (_randomPowerUp == 5 && Random.value < 0.25)
            {
                Instantiate(_powerUps[_randomPowerUp], _positionToSpawn, Quaternion.identity);
            }
            else if (_randomPowerUp != 5)
            {
                Instantiate(_powerUps[_randomPowerUp], _positionToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(3, 11));
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
